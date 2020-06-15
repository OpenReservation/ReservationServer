using OpenReservation.API.Test.MockServices;
using OpenReservation.Common;
using OpenReservation.Database;
using OpenReservation.Events;
using OpenReservation.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WeihanLi.Common;
using WeihanLi.Common.Event;
using WeihanLi.Redis;
using WeihanLi.Web.Authentication;
using WeihanLi.Web.Authentication.HeaderAuthentication;

namespace OpenReservation.API.Test
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCaching();

            services.AddControllers(options =>
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
                        NoStore = true
                    });
                })
                .AddApplicationPart(typeof(ApiControllerBase).Assembly)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc; // 设置时区为 UTC
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // addDbContext
            services.AddDbContextPool<ReservationDbContext>(options => options.UseInMemoryDatabase("Reservation"));

            services.AddGoogleRecaptchaHelper(Configuration.GetSection("GoogleRecaptcha"));
            services.AddTencentCaptchaHelper(options =>
            {
                options.AppId = Configuration["Tencent:Captcha:AppId"];
                options.AppSecret = Configuration["Tencent:Captcha:AppSecret"];
            });

            // registerApplicationSettingService
            services.TryAddSingleton<IApplicationSettingService, ApplicationSettingInMemoryService>();
            // register access control service
            services.AddAccessControlHelper<AdminPermissionRequireStrategy, AdminOnlyControlAccessStrategy>();

            services.AddEvents()
                .AddEventHandler<NoticeViewEvent, NoticeViewEventHandler>()
                .AddEventHandler<OperationLogEvent, OperationLogEventHandler>()
                ;

            services.TryAddSingleton<ICacheClient, MockRedisCacheClient>();

            // RegisterAssemblyModules
            services.RegisterAssemblyModules();

            services
                .AddAuthentication(HeaderAuthenticationDefaults.AuthenticationSchema)
                .AddHeader()
                //.AddAuthentication(QueryAuthenticationDefaults.AuthenticationSchema)
                //.AddQuery()
                ;
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ReservationApi", builder => builder.RequireAuthenticatedUser());
            });

            // SetDependencyResolver
            DependencyResolver.SetDependencyResolver(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseResponseCaching();

            app.UseRouting();

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            TestDataInitializer.Initialize(app.ApplicationServices);
        }
    }
}
