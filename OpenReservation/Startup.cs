using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using OpenReservation.AuditEnrichers;
using OpenReservation.Common;
using OpenReservation.Database;
using OpenReservation.Events;
using OpenReservation.Extensions;
using OpenReservation.Helpers;
using OpenReservation.Services;
using OpenReservation.ViewModels;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using StackExchange.Redis;
using WeihanLi.Common;
using WeihanLi.Common.Event;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Http;
using WeihanLi.Common.Logging;
using WeihanLi.Common.Logging.Serilog;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration.ReplacePlaceholders();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; // 设置时区为 UTC
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

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
                    })
                .AddCookie(options =>
                {
                    //options.LoginPath = "/Admin/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.LogoutPath = "/Account/LogOut";

                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
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

            // addDbContext
            services.AddDbContextPool<ReservationDbContext>(option =>
            {
                var dbType = Configuration.GetAppSetting("DbType");
                if ("InMemory".EqualsIgnoreCase(dbType))
                {
                    option.UseInMemoryDatabase("Reservation");
                }
                else if ("MySql".EqualsIgnoreCase(dbType))
                {
                    option.UseMySql(Configuration.GetConnectionString("Reservation"));
                }
                else
                {
                    option.UseSqlServer(Configuration.GetConnectionString("Reservation"));
                }
            }, 100);

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

            // registerApplicationSettingService
            services.TryAddSingleton<IApplicationSettingService, ApplicationSettingInRedisService>();

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
                );
            });

            // register access control service
            services.AddAccessControlHelper()
                .AddResourceAccessStrategy<AdminPermissionRequireStrategy>()
                .AddControlAccessStrategy<AdminOnlyControlAccessStrategy>()
                ;

            services.TryAddSingleton<NoProxyHttpClientHandler>();

            services.AddHttpClient<ChatBotHelper>(client =>
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                })
                .ConfigurePrimaryHttpMessageHandler(() => new NoProxyHttpClientHandler());
            services.TryAddSingleton<ChatBotHelper>();

            services.AddHttpClient<WechatAPI.Helper.WeChatHelper>()
                .ConfigurePrimaryHttpMessageHandler(() => new NoProxyHttpClientHandler());
            services.TryAddSingleton<WechatAPI.Helper.WeChatHelper>();

            services.AddRedisConfig(options =>
            {
                options.RedisServers = new[]
                {
                    new RedisServerConfiguration(Configuration.GetConnectionString("Redis")  ?? "127.0.0.1"),
                };
                options.CachePrefix = "OpenReservation"; //  ApplicationHelper.ApplicationName by default
                options.DefaultDatabase = 2;
            });

            // DataProtection persist in redis
            services.AddDataProtection()
                .SetApplicationName(ApplicationHelper.ApplicationName)
                .PersistKeysToStackExchangeRedis(() => DependencyResolver.Current.ResolveService<IConnectionMultiplexer>().GetDatabase(5), "DataProtection-Keys")
                ;

            // events
            services.AddEvents()
                .AddEventHandler<NoticeViewEvent, NoticeViewEventHandler>()
                .AddEventHandler<OperationLogEvent, OperationLogEventHandler>()
                ;

            // services.AddHostedService<RemoveOverdueReservationService>();
            // services.AddHostedService<CronLoggingTest>();

            services.Configure<CustomExceptionHandlerOptions>(options =>
            {
                options.OnRequestAborted = (context, logger) => Task.CompletedTask;

                options.OnException = (context, logger, exception) =>
                {
                    if (exception is TaskCanceledException || exception is OperationCanceledException)
                    {
                        return Task.CompletedTask;
                    }
                    if (exception is AggregateException aggregateException)
                    {
                        var ex = aggregateException.Unwrap();
                        if (ex is TaskCanceledException || ex is OperationCanceledException)
                        {
                            return Task.CompletedTask;
                        }
                    }

                    logger.LogError(exception, exception.Message);
                    return Task.CompletedTask;
                };
            });

            // gitee storage
            services.AddGiteeStorageProvider(Configuration.GetSection("Storage:Gitee"));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(ApplicationHelper.ApplicationName, new Microsoft.OpenApi.Models.OpenApiInfo { Title = "活动室预约系统 API", Version = "1.0" });

                options.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, $"{typeof(Models.Notice).Assembly.GetName().Name}.xml"));
                options.IncludeXmlComments(System.IO.Path.Combine(AppContext.BaseDirectory, $"{typeof(API.NoticeController).Assembly.GetName().Name}.xml"), true);
            });

            services.AddHttpContextUserIdProvider(options =>
            {
                options.UserIdFactory = context =>
                {
                    var user = context?.User;
                    if (null != user && user.Identity.IsAuthenticated)
                    {
                        return $"{user.GetUserId<string>()}--{user.Identity.Name}";
                    }

                    var userIp = context?.GetUserIP();
                    if (null != userIp)
                    {
                        return userIp;
                    }

                    return $"{Environment.MachineName}__{Environment.UserName}";
                };
            });

            // RegisterAssemblyModules
            services.RegisterAssemblyModules();

            // SetDependencyResolver
            DependencyResolver.SetDependencyResolver(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IEventBus eventBus)
        {
            app.UseCustomExceptionHandler();
            app.UseHealthCheck("/health");
            app.UseRequestLocalization();

            app.UseStaticFiles();

            app.UseResponseCaching();
            app.UseResponseCompression();

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    // c.RoutePrefix = string.Empty; //
                    c.SwaggerEndpoint($"/swagger/{ApplicationHelper.ApplicationName}/swagger.json", "活动室预约系统 API");
                    c.DocumentTitle = "活动室预约系统 API";
                });

            app.UseRouting();

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            if (env.IsDevelopment())
            {
                app.UseRequestLog();
                app.UsePerformanceLog();
            }

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

            // initialize settings

            #region Logging Configure

            LogHelper.ConfigureLogging(builder =>
                {
                    builder.AddSerilog(loggingConfig =>
                        {
                            loggingConfig
                                .Enrich.FromLogContext()
                                .Enrich.WithHttpContextInfo(app.ApplicationServices, (logEvent, propertyFactory, httpContext) =>
                                    {
                                        logEvent.AddPropertyIfAbsent(
                                            propertyFactory.CreateProperty("RequestIP", httpContext.GetUserIP()));
                                        logEvent.AddPropertyIfAbsent(
                                            propertyFactory.CreateProperty("RequestPath", httpContext.Request.Path));
                                        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestMethod",
                                            httpContext.Request.Method));

                                        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Referer",
                                            httpContext.Request.Headers["Referer"].ToString()));
                                        if (httpContext.Response.HasStarted)
                                        {
                                            logEvent.AddPropertyIfAbsent(
                                                propertyFactory.CreateProperty("ResponseStatus",
                                                    httpContext.Response.StatusCode));
                                        }
                                    })
                                ;

                            var esConnString = Configuration.GetConnectionString("ElasticSearch");
                            if (esConnString.IsNotNullOrWhiteSpace())
                            {
                                loggingConfig.WriteTo.Elasticsearch(esConnString, $"logstash-{ApplicationHelper.ApplicationName.ToLower()}");
                            }
                        })
                        .WithFilter((providerType, categoryName, logLevel, exception) =>
                        {
                            if (exception != null)
                            {
                                var ex = exception.Unwrap();
                                if (ex is TaskCanceledException || ex is OperationCanceledException)
                                {
                                    return false;
                                }
                            }

                            if ((categoryName.StartsWith("Microsoft") || categoryName.StartsWith("System")) &&
                                logLevel <= LogHelperLogLevel.Info)
                            {
                                return false;
                            }

                            return true;
                        });
                });

            loggerFactory
                .AddSerilog()
                .AddSentry(options =>
                {
                    options.Dsn = Configuration.GetAppSetting("SentryClientKey");
                    options.Environment = env.EnvironmentName;
                    options.MinimumEventLevel = LogLevel.Error;
                    options.Debug = env.IsDevelopment();

                    options.BeforeSend = (sentryEvent) =>
                    {
                        // ignore TaskCanceledException/OperationCanceledException
                        if (sentryEvent.Exception is TaskCanceledException ||
                            sentryEvent.Exception is OperationCanceledException)
                        {
                            return null;
                        }

                        return sentryEvent;
                    };
                });

            #endregion Logging Configure

            // init data
            app.ApplicationServices.Initialize();
            EFAuditConfig(app);
            ExcelSettings();
        }

        private void EFAuditConfig(IApplicationBuilder applicationBuilder)
        {
            var userIdProvider = applicationBuilder.ApplicationServices
                .GetRequiredService<IUserIdProvider>();
            AuditConfig.Configure(builder =>
            {
                builder
                    .EnrichWithProperty(nameof(ApplicationHelper.ApplicationName),
                        ApplicationHelper.ApplicationName)
                    .EnrichWithProperty("Host", Environment.MachineName)
                    .WithUserIdProvider(userIdProvider)
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
