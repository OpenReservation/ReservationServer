using System.Net;
using System.Globalization;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenReservation.Common;
using OpenReservation.Database;
using OpenReservation.Events;
using OpenReservation.ExcelMappingProfiles;
using OpenReservation.Helpers;
using OpenReservation.Models;
using OpenReservation.Services;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;
using StackExchange.Redis;
using System.Diagnostics.Metrics;
using WeihanLi.Common;
using WeihanLi.Common.Event;
using WeihanLi.Common.Helpers;
using WeihanLi.EntityFramework;
using WeihanLi.EntityFramework.Audit;
using WeihanLi.EntityFramework.Interceptors;
using WeihanLi.Extensions;
using WeihanLi.Extensions.Localization.Json;
using WeihanLi.Npoi;
using WeihanLi.Redis;
using WeihanLi.Web.Extensions;
using WeihanLi.Web.Middleware;

namespace OpenReservation;

public class Startup(IConfiguration configuration, IWebHostEnvironment environment)
{
    private static readonly Counter<int> ExceptionCounter = Helpers.DiagnosticHelper.Meter.CreateCounter<int>("Unhandled_exception", "{count}", "Unhandled Exception");

    public IConfiguration Configuration { get; } = configuration.ReplacePlaceholders();

    public IWebHostEnvironment Environment { get; } = environment;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            options.Secure = CookieSecurePolicy.SameAsRequest;
            options.OnAppendCookie = cookieContext =>
                AuthenticationHelper.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            options.OnDeleteCookie = cookieContext =>
                AuthenticationHelper.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
        });

        services.AddHealthChecks();

        services.AddJsonLocalization(options =>
        {
            options.ResourcesPath = Configuration.GetAppSetting("ResourcesPath");
            options.ResourcesPathType = ResourcesPathType.CultureBased;
        });

        services.AddResponseCaching();
        services.AddResponseCompression();

        services.AddControllersWithViews(options =>
            {
                options.CacheProfiles.Add("default", new CacheProfile()
                {
                    Duration = 300,
                    VaryByQueryKeys = new[] { "*" }
                });
                options.CacheProfiles.Add("private", new CacheProfile()
                {
                    Duration = 300,
                    Location = ResponseCacheLocation.Client,
                    VaryByQueryKeys = new[] { "*" }
                });
                options.CacheProfiles.Add("noCache", new CacheProfile()
                {
                    Duration = null,
                    NoStore = true
                });
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            })
            .AddViewLocalization()
            .AddDataAnnotationsLocalization()
            ;

        var supportedCultureNames = Configuration.GetSection("Localization:SupportedCultures")?.Get<string[]>();
        if (supportedCultureNames is not { Length: not 0 })
        {
            supportedCultureNames = ["zh", "en"];
        }
        var supportedCultures = supportedCultureNames.Select(name => new CultureInfo(name)).ToArray();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(supportedCultures[0].Name);
            // Formatting numbers, dates, etc.
            options.SupportedCultures = supportedCultures;
            // UI strings that we have localized.
            options.SupportedUICultures = supportedCultures;
        });

        //Cookie Authentication
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                //options.LoginPath = "/Admin/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.LogoutPath = "/Account/LogOut";

                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            })
            .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Authority = Configuration["Authorization:Authority"];
                options.RequireHttpsMetadata = false;

                options.NameClaimType = "name";
                options.RoleClaimType = "role";
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                var authorizationConfiguration = Configuration.GetSection("Authorization");
                authorizationConfiguration.Bind(options);

                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;

                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClaimActions.MapJsonKey("role", "role");

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                };
                options.Events.OnMessageReceived = context =>
                {
                    if (context.Properties is not null)
                    {
                        context.Properties.IsPersistent = true;
                    }
                    
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToIdentityProvider = rc =>
                {
                    rc.ProtocolMessage.RedirectUri = authorizationConfiguration["RedirectUri"];
                    return Task.CompletedTask;
                };
                options.Events.OnUserInformationReceived = context =>
                {
                    return Task.CompletedTask;
                };
            })
            ;
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ReservationManager", builder => builder
                .AddAuthenticationSchemes(OpenIdConnectDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireRole("ReservationManager", "ReservationAdmin")
            );
            options.AddPolicy("ReservationAdmin", builder => builder
                .AddAuthenticationSchemes(OpenIdConnectDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireRole("ReservationAdmin")
            );
            options.AddPolicy("ReservationApi", builder => builder
                .AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireScope("ReservationApi")
            );
        });

        // addDbContext
        services.AddDbContext<ReservationDbContext>((provider, options) =>
        {
            options.AddInterceptors
            ([
                provider.GetRequiredService<AutoUpdateInterceptor>(),
                provider.GetRequiredService<AuditInterceptor>()
            ]);
            var dbType = Configuration.GetAppSetting<DbType>("DbType");
            var connectionString = Configuration.GetConnectionString("Reservation");
            switch (dbType)
            {
                case DbType.InMemory:
                    options.UseInMemoryDatabase("Reservation");
                    break;

                case DbType.Sqlite:
                    options.UseSqlite(connectionString ?? "Data Source=Reservation.db;Cache=Shared");
                    break;
                
                case DbType.Npgsql:
                    options.UseNpgsql(connectionString);
                    break;

                default:
                    options.UseSqlServer(connectionString);
                    break;
            }
        });
        services.AddEFAutoUpdateInterceptor();
        services.AddEFAutoAudit(_ =>
        {
        });

        services.AddGoogleRecaptchaHelper(Configuration.GetSection("GoogleRecaptcha"), client =>
        {
            client.Timeout = TimeSpan.FromSeconds(3);
        });
        services.AddTencentCaptchaHelper(options =>
        {
            options.AppId = Configuration["Tencent:Captcha:AppId"];
            options.AppSecret = Configuration["Tencent:Captcha:AppSecret"];
        }, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(3);
        });
        services.AddHttpClient<ChatBotHelper>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(5);
        }).AddTransientHttpErrorPolicy(builder => builder.RetryAsync(5));
        services.TryAddSingleton<ChatBotHelper>();
        services.AddHttpClient<WechatAPI.Helper.WeChatHelper>();
        services.TryAddSingleton<WechatAPI.Helper.WeChatHelper>();

        // registerApplicationSettingService
        if (Environment.IsDevelopment())
        {
            services.TryAddSingleton<IApplicationSettingService, ApplicationSettingInMemoryService>();
        }
        else
        {
            services.TryAddSingleton<IApplicationSettingService, ApplicationSettingInRedisService>();
        }

        // register access control service
        services.AddAccessControlHelper()
            .AddResourceAccessStrategy<AdminPermissionRequireStrategy>()
            .AddControlAccessStrategy<AdminOnlyControlAccessStrategy>()
            ;

        var redisConfiguration = ConfigurationOptions.Parse(Configuration.GetRequiredConnectionString("Redis"));
        var redisServers = redisConfiguration.EndPoints.Select(e => 
        {
            return e switch
            {
                DnsEndPoint dnsEndPoint => new RedisServerConfiguration(dnsEndPoint.Host, dnsEndPoint.Port > 0 ? dnsEndPoint.Port : 6379),
                IPEndPoint ipEndPoint => new RedisServerConfiguration(ipEndPoint.Address.ToString(), ipEndPoint.Port > 0 ? ipEndPoint.Port : 6379),
                _ => throw new ArgumentException("Invalid redis configuration")
            };
        }).ToArray();
        services.AddRedisConfig(options =>
        {
            options.Password = redisConfiguration.Password;
            options.DefaultDatabase = 0;
            options.CachePrefix = "OpenReservation";
            options.RedisServers = redisServers;
        });

        // DataProtection persist in redis
        var dataProtectionBuilder = services.AddDataProtection()
            .SetApplicationName(ApplicationHelper.ApplicationName);
        if (!Environment.IsDevelopment())
        {
            dataProtectionBuilder.PersistKeysToStackExchangeRedis(
                () => DependencyResolver.Current
                    .ResolveService<IDatabase>(),
                "DataProtection-Keys");
        }

        // events
        services.AddEvents()
            .AddEventHandler<NoticeViewEvent, NoticeViewEventHandler>()
            .AddEventHandler<OperationLogEvent, OperationLogEventHandler>()
            ;

        services.Configure<CustomExceptionHandlerOptions>(options =>
        {
            options.OnRequestAborted = (_, _) => Task.CompletedTask;

            options.OnException = (context, logger, exception) =>
            {
                var ex = exception;
                if (exception is AggregateException aggregateException)
                {
                    ex = aggregateException.Unwrap();
                }

                if (context.RequestAborted.IsCancellationRequested && ex is TaskCanceledException or OperationCanceledException
                   )
                {
                    return Task.CompletedTask;
                }

                logger.LogError(exception, exception.Message);
                ExceptionCounter.Add(1, new KeyValuePair<string, object>("type", ex.GetType().Name));

                return Task.CompletedTask;
            };
        });

        // gitee storage
        services.AddGiteeStorageProvider(Configuration.GetSection("Storage:Gitee"));

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(ApplicationHelper.ApplicationName, new OpenApiInfo { Title = "活动室预约系统 API", Version = "1.0" });

            options.IncludeXmlComments(typeof(Notice).Assembly);
            options.IncludeXmlComments(typeof(API.NoticeController).Assembly, true);
            // Add security definitions
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, Array.Empty<string>()
                }
            });
        });

        services.AddHttpContextUserIdProvider(options =>
        {
            options.UserIdFactory = context =>
            {
                if (context?.User.Identity?.IsAuthenticated == true)
                {
                    return $"{context.User.GetUserId()}--{context.User.Identity.Name}";
                }

                var userIp = context?.GetUserIP();
                if (null != userIp)
                {
                    return userIp;
                }

                return $"{System.Environment.MachineName}__{System.Environment.UserName}";
            };
        });

        services.AddHostedService<TimedHealthCheckService>();
        // RegisterAssemblyModules
        services.RegisterAssemblyModules();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IEventBus eventBus)
    {
        DependencyResolver.SetDependencyResolver(app.ApplicationServices);
        app.UseCookiePolicy();

        app.UseCustomExceptionHandler();
        app.UseHealthCheck("/health");

        app.UseStaticFiles();
        app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                // c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint($"/swagger/{ApplicationHelper.ApplicationName}/swagger.json", "活动室预约系统 API");
                c.DocumentTitle = "OpenReservation API";
            });

        app.UseRequestLocalization();
        app.UseResponseCaching();

        app.UseHttpLogging();

        app.UseRouting();
        app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true));

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapControllerRoute("Notice", "/Notice/{path}.html", new
            {
                controller = "Home",
                action = "NoticeDetails"
            });
            endpoints.MapControllerRoute(name: "areaRoute", "{area:exists}/{controller=Home}/{action=Index}");
            endpoints.MapDefaultControllerRoute();
        });

        // init data
        app.ApplicationServices.Initialize();

        // initialize settings
        LoggingConfig(loggerFactory);
        ExcelSettings();
    }

    private void LoggingConfig(ILoggerFactory loggerFactory)
    {
        loggerFactory
            .AddSentry(options =>
            {
                options.Dsn = Configuration.GetAppSetting("SentryClientKey");
                options.Environment = Environment.EnvironmentName;
                options.MinimumEventLevel = LogLevel.Error;
                options.Debug = Environment.IsDevelopment();
                options.SetBeforeSend(sentryEvent =>
                {
                    // ignore TaskCanceledException/OperationCanceledException
                    if (sentryEvent.Exception is OperationCanceledException or TaskCanceledException)
                    {
                        return null;
                    }

                    return sentryEvent;
                });
            });
    }

    private static void ExcelSettings()
    {
        FluentSettings.LoadMappingProfiles(typeof(ReservationListMappingProfile).Assembly);
    }
}
