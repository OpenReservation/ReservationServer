using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenReservation.AuditEnrichers;
using OpenReservation.Common;
using OpenReservation.Database;
using OpenReservation.Events;
using OpenReservation.Helpers;
using OpenReservation.Models;
using OpenReservation.Services;
using OpenReservation.ViewModels;
using Polly;
using Prometheus;
using StackExchange.Redis;
using WeihanLi.Common;
using WeihanLi.Common.Event;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Services;
using WeihanLi.EntityFramework.Audit;
using WeihanLi.Extensions;
using WeihanLi.Extensions.Localization.Json;
using WeihanLi.Npoi;
using WeihanLi.Redis;
using WeihanLi.Web.Extensions;
using WeihanLi.Web.Middleware;

namespace OpenReservation
{
    public class Startup
    {
        private static readonly Counter ExceptionCounter = Metrics.CreateCounter("Unhandled_exception", "Unhandled Exception", "error");

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration.ReplacePlaceholders();
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

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
            if (supportedCultureNames == null || supportedCultureNames.Length == 0)
            {
                supportedCultureNames = new[] { "zh", "en" };
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
                            context.Properties.IsPersistent = true;
                            return Task.CompletedTask;
                        };
                    options.Events.OnRedirectToIdentityProvider = rc =>
                        {
                            rc.ProtocolMessage.RedirectUri = authorizationConfiguration["RedirectUri"];
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
            services.AddDbContextPool<ReservationDbContext>(option =>
            {
                var dbType = Configuration.GetAppSetting("DbType");
                if ("InMemory".EqualsIgnoreCase(dbType))
                {
                    option.UseInMemoryDatabase("Reservation");
                }
                else if ("Sqlite".EqualsIgnoreCase(dbType))
                {
                    option.UseSqlite("Data Source=Reservation.db;Cache=Shared");
                }
                else if ("MySql".EqualsIgnoreCase(dbType))
                {
                    option.UseMySql(Configuration.GetConnectionString("Reservation"));
                }
                else
                {
                    option.UseSqlServer(Configuration.GetConnectionString("Reservation"));
                }
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

            services.AddRedisConfig(options =>
            {
                options.DefaultDatabase = 0;
                options.RedisServers = new[]
                {
                    new RedisServerConfiguration(Configuration.GetConnectionString("Redis")  ?? "127.0.0.1"),
                };
                options.CachePrefix = "OpenReservation";
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

                    if (context.RequestAborted.IsCancellationRequested && (
                        ex is TaskCanceledException || ex is OperationCanceledException)
                        )
                    {
                        return Task.CompletedTask;
                    }

                    logger.LogError(exception, exception.Message);

                    ExceptionCounter.WithLabels(exception.Message).Inc();

                    return Task.CompletedTask;
                };
            });

            // gitee storage
            services.AddGiteeStorageProvider(Configuration.GetSection("Storage:Gitee"));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(ApplicationHelper.ApplicationName, new OpenApiInfo { Title = "活动室预约系统 API", Version = "1.0" });

                options.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, $"{typeof(Notice).Assembly.GetName().Name}.xml"));
                options.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, $"{typeof(API.NoticeController).Assembly.GetName().Name}.xml"), true);
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

            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true));
            app.UseHttpMetrics();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
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
            EFAuditConfig(app);
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

                    options.BeforeSend = (sentryEvent) =>
                    {
                        // ignore TaskCanceledException/OperationCanceledException
                        if (sentryEvent.Exception is OperationCanceledException or TaskCanceledException)
                        {
                            return null;
                        }

                        return sentryEvent;
                    };
                });
        }

        private void EFAuditConfig(IApplicationBuilder applicationBuilder)
        {
            var userIdProvider = applicationBuilder.ApplicationServices
                .GetRequiredService<IUserIdProvider>();
            AuditConfig.Configure(builder =>
            {
                builder
                    .EnrichWithProperty(nameof(ApplicationHelper.ApplicationName), ApplicationHelper.ApplicationName)
                    .EnrichWithProperty("Host", System.Environment.MachineName)
                    .WithUserIdProvider(userIdProvider)
                    .IgnoreEntity<OperationLog>()
                    .WithHttpContextInfo(applicationBuilder.ApplicationServices.GetRequiredService<IHttpContextAccessor>())
                    ;
            });
        }

        private void ExcelSettings()
        {
            var settings = FluentSettings.For<ReservationListViewModel>();

            settings
                .HasAuthor("WeihanLi")
                .HasTitle("活动室预约信息")
                .HasDescription("活动室预约信息")
                .HasSheetConfiguration(0, "活动室预约信息", true)
                ;

            settings.Property(r => r.ReservationId).Ignored();

            settings.Property(r => r.ReservationPlaceName)
                .HasColumnTitle("活动室名称")
                .HasColumnIndex(0);
            settings.Property(r => r.ReservationForDate)
                .HasColumnTitle("预约使用日期")
                .HasColumnIndex(1);
            settings.Property(r => r.ReservationForTime)
                .HasColumnTitle("预约使用的时间段")
                .HasColumnIndex(2);
            settings.Property(r => r.ReservationUnit)
                .HasColumnTitle("预约单位")
                .HasColumnIndex(3);
            settings.Property(r => r.ReservationActivityContent)
                .HasColumnTitle("预约活动内容")
                .HasColumnIndex(4);
            settings.Property(r => r.ReservationPersonName)
                .HasColumnTitle("预约人姓名")
                .HasColumnIndex(5);
            settings.Property(r => r.ReservationPersonPhone)
                .HasColumnTitle("预约人手机号")
                .HasColumnIndex(6);
            settings.Property(r => r.ReservationTime)
                .HasColumnTitle("预约时间")
                .HasColumnFormatter("yyyy-MM-dd HH:mm:ss")
                .HasColumnIndex(7);
            settings.Property(r => r.ReservationStatus)
                .HasColumnTitle("审核状态")
                .HasColumnOutputFormatter(status => status.GetDescription())
                .HasColumnIndex(8);
        }
    }
}
