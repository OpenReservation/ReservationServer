using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenReservation.API.Test.MockServices;
using OpenReservation.Common;
using OpenReservation.Database;
using OpenReservation.Events;
using OpenReservation.Services;
using WeihanLi.Common.Event;
using WeihanLi.Common.Helpers;
using WeihanLi.Redis;
using WeihanLi.Web.Authentication;
using WeihanLi.Web.Authentication.HeaderAuthentication;

namespace OpenReservation.API.Test
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            var baseUrl = $"http://localhost:{NetHelper.GetRandomPort()}";

            hostBuilder
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseUrls(baseUrl);
                    builder.ConfigureServices((context, services) =>
                    {
                        services.TryAddSingleton<APITestFixture>();
                        services.TryAddSingleton(new HttpClient()
                        {
                            BaseAddress = new Uri(baseUrl)
                        });

                        ConfigureServices(services, context.Configuration);
                    });
                    builder.Configure(Configure);
                })
                ;
        }

        private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
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

            services.AddGoogleRecaptchaHelper(configuration.GetSection("GoogleRecaptcha"));
            services.AddTencentCaptchaHelper(options =>
            {
                options.AppId = configuration["Tencent:Captcha:AppId"];
                options.AppSecret = configuration["Tencent:Captcha:AppSecret"];
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
        }

        private void Configure(IApplicationBuilder app)
        {
            app.UseResponseCaching();

            app.UseRouting();

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
