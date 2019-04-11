using ActivityReservation.Business;
using ActivityReservation.Common;
using ActivityReservation.Database;
using ActivityReservation.Helpers;
using ActivityReservation.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
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
using WeihanLi.Common;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Logging;
using WeihanLi.Redis;

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
            services.AddMemoryCache();
            // services.AddDistributedRedisCache(options => options.Configuration = Configuration.GetConnectionString("Redis"));

            services.AddSession();
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // TODO: DataProtection persist in redis

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
#if !DEBUG
                options.RedisServers = new[]
                {
                    new RedisServerConfiguration(Configuration.GetConnectionString("Redis")),
                };
#endif
                options.CachePrefix = "ActivityReservation";
                options.DefaultDatabase = 2;
            });

            services.Configure<GeetestOptions>(Configuration.GetSection("Geetest"));
            services.AddGeetestHelper();
            services.Configure<GoogleRecaptchaOptions>(Configuration.GetSection("GoogleRecaptcha"));
            services.AddGoogleRecaptchaHelper();

            services.AddBLL();
            services.AddSingleton<OperLogHelper>();
            services.AddScoped<ReservationHelper>();
            // registerApplicationSettingService
            services.TryAddSingleton<IApplicationSettingService, ApplicationSettingInRedisService>();
            // register access control service
            services.AddAccessControlHelper<Filters.AdminPermissionRequireStrategy, Filters.AdminOnlyControlAccessStragety>();

            // SetDependencyResolver
            DependencyResolver.SetDependencyResolver(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            LogHelper.AddLogProvider(new ILogHelperProvider[] {
                new WeihanLi.Common.Logging.Log4Net.Log4NetLogHelperProvider(),
                new Common.SentryLogHelperProvider(),
            });
            loggerFactory
                .AddLog4Net()
                .AddSentry(Configuration.GetAppSetting("SentryClientKey"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
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
    }
}
