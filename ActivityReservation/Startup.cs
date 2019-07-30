using System;
using System.IO;
using System.Threading.Tasks;
using ActivityReservation.API;
using ActivityReservation.Business;
using ActivityReservation.Common;
using ActivityReservation.Database;
using ActivityReservation.Events;
using ActivityReservation.Extensions;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.Services;
using ActivityReservation.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;
using WeihanLi.Common;
using WeihanLi.Common.Event;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Http;
using WeihanLi.Common.Logging.Serilog;
using WeihanLi.EntityFramework;
using WeihanLi.Extensions;
using WeihanLi.Npoi;
using WeihanLi.Redis;
using WeihanLi.Web.Extensions;
using WeihanLi.Web.Middlewares;

namespace ActivityReservation
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

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; // 设置时区为 UTC
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // DataProtection persist in redis
            services.AddDataProtection()
                .SetApplicationName(ApplicationHelper.ApplicationName)
                .PersistKeysToStackExchangeRedis(() => DependencyResolver.Current.ResolveService<IConnectionMultiplexer>().GetDatabase(5), "DataProtection-Keys")
                ;

            //Cookie Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/Admin/Account/Login";
                    options.LoginPath = "/Admin/Account/Login";
                    options.LogoutPath = "/Admin/Account/LogOut";

                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                });

            // addDbContext
            services.AddDbContextPool<ReservationDbContext>(option => option.UseMySql(Configuration.GetConnectionString("Reservation")));
            services.AddRedisConfig(options =>
            {
                options.RedisServers = new[]
                {
#if DEBUG
                    new RedisServerConfiguration("127.0.0.1"),
#else
                    new RedisServerConfiguration(Configuration.GetConnectionString("Redis")),
#endif
                };
                options.CachePrefix = "ActivityReservation"; //  ApplicationHelper.ApplicationName by default
                options.DefaultDatabase = 2;
            });

            services.AddHttpClient<GoogleRecaptchaHelper>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(3);
            });
            services.Configure<GoogleRecaptchaOptions>(Configuration.GetSection("GoogleRecaptcha"));
            services.AddGoogleRecaptchaHelper();

            services.AddHttpClient<TencentCaptchaHelper>(client => client.Timeout = TimeSpan.FromSeconds(3))
                .ConfigurePrimaryHttpMessageHandler(() => new NoProxyHttpClientHandler());
            services.AddTencentCaptchaHelper(options =>
            {
                options.AppId = Configuration["Tencent:Captcha:AppId"];
                options.AppSecret = Configuration["Tencent:Captcha:AppSecret"];
            });

            services.AddEFRepository();
            services.AddBLL();

            services.AddSingleton<OperLogHelper>();
            services.AddScoped<ReservationHelper>();

            // registerApplicationSettingService
            services.TryAddSingleton<IApplicationSettingService, ApplicationSettingInRedisService>();
            // register access control service
            services.AddAccessControlHelper<Filters.AdminPermissionRequireStrategy, Filters.AdminOnlyControlAccessStrategy>();

            services.AddHttpClient<ChatBotHelper>(client =>
                {
                    client.Timeout = TimeSpan.FromSeconds(3);
                })
                .ConfigurePrimaryHttpMessageHandler(() => new NoProxyHttpClientHandler());
            services.TryAddSingleton<ChatBotHelper>();

            services.AddHttpClient<WechatAPI.Helper.WechatHelper>()
                .ConfigurePrimaryHttpMessageHandler(() => new NoProxyHttpClientHandler());
            services.TryAddSingleton<WechatAPI.Helper.WechatHelper>();

            services.TryAddSingleton<CaptchaVerifyHelper>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(ApplicationHelper.ApplicationName, new Info { Title = "活动室预约系统 API", Version = "1.0" });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Notice).Assembly.GetName().Name}.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(NoticeController).Assembly.GetName().Name}.xml"), true);
            });

            services.AddSingleton<IEventBus, RedisEventBus>();
            services.AddSingleton<IEventStore, EventStoreInRedis>();
            //register EventHandlers
            services.AddSingleton<OperationLogEventHandler>();
            services.AddSingleton<NoticeViewEventHandler>();

            services.AddHostedService<RemoveOverdueReservationService>();

            services.Configure<CustomExceptionHandlerOptions>(options =>
                {
                    options.OnException = (context, logger, exception) =>
                    {
                        if (exception is TaskCanceledException || exception is OperationCanceledException)
                        {
                            return Task.CompletedTask;
                        }
                        logger.LogError(exception, exception.Message);
                        return Task.CompletedTask;
                    };
                });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
                options.ForwardLimit = null;
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All;
            });

            // SetDependencyResolver
            DependencyResolver.SetDependencyResolver(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IEventBus eventBus)
        {
            FluentExcelSettings();

            eventBus.Subscribe<OperationLogEvent, OperationLogEventHandler>(); // 注册操作日志 Event
            eventBus.Subscribe<NoticeViewEvent, NoticeViewEventHandler>(); // 公告

            LogHelper.LogFactory.AddSerilog(loggingConfig =>
                {
                    loggingConfig.WriteTo.Elasticsearch(Configuration.GetConnectionString("ElasticSearch"), $"logstash-{ApplicationHelper.ApplicationName.ToLower()}");

                    loggingConfig.Enrich.When(logEvent => logEvent.Properties["SourceContext"].ToString() == "RequestLog", config => config.WithRequestInfo());
                });

            loggerFactory
                .AddSerilog()
                .AddSentry(options =>
                {
                    options.Dsn = Configuration.GetAppSetting("SentryClientKey");
                    options.Environment = env.EnvironmentName;
                    options.MinimumEventLevel = LogLevel.Error;
                });

            app.UseForwardedHeaders();
            app.UseCustomExceptionHandler();
            app.UseHealthCheck("/health");

            app.UseStaticFiles();
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    // c.RoutePrefix = string.Empty; //
                    c.SwaggerEndpoint($"/swagger/{ApplicationHelper.ApplicationName}/swagger.json", "活动室预约系统 API");
                    c.DocumentTitle = "活动室预约系统 API";
                });

            app.UseRequestLog();
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("Notice", "/Notice/{path}.html", new
                {
                    controller = "Home",
                    action = "NoticeDetails"
                });

                routes.MapRoute(name: "areaRoute",
                  template: "{area:exists}/{controller=Home}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            // initialize
            app.ApplicationServices.Initialize();
        }

        private void FluentExcelSettings()
        {
            //
            var settings = ExcelHelper.SettingFor<ReservationListViewModel>();
            settings.HasAuthor("WeihanLi")
                .HasTitle("活动室预约信息")
                .HasDescription("活动室预约信息");
            settings.Property(r => r.ReservationId).Ignored();
            settings.Property(r => r.ReservationForDate)
                .HasColumnTitle("预约使用日期");
            settings.Property(r => r.ReservationForTime)
                .HasColumnTitle("预约使用的时间段");
            settings.Property(r => r.ReservationUnit)
                .HasColumnTitle("预约单位");
            settings.Property(r => r.ReservationTime)
                .HasColumnTitle("预约时间")
                .HasColumnFormatter("yyyy-MM-dd HH:mm:ss");
            settings.Property(r => r.ReservationPersonName)
                .HasColumnTitle("预约人姓名");
            settings.Property(r => r.ReservationPersonPhone)
                .HasColumnTitle("预约人手机号");
            settings.Property(r => r.ReservationActivityContent)
                .HasColumnTitle("预约活动内容");
            settings.Property(r => r.ReservationPlaceName)
                .HasColumnTitle("活动室名称");
            settings.Property(r => r.ReservationStatus)
                .HasColumnTitle("审核状态")
                .HasColumnFormatter((entity, propertyVal) => propertyVal.GetDescription());
        }
    }
}
