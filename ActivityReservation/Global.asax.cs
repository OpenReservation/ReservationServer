using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ActivityReservation.Common;
using ActivityReservation.Controllers;
using Autofac;
using Autofac.Integration.Mvc;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Log;
using WeihanLi.Redis;

namespace ActivityReservation
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //log4net init
            LogHelper.LogInit(new ILogHelperProvider[]
            {
                new SentryLogHelperProvider()
            });

            //Register filters
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            #region DependenceInjection

            var builder = new ContainerBuilder();

            //RegisterAssemblyModules
            builder.RegisterAssemblyModules(BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray());

            //ReservationDbContext
            builder.RegisterType<Models.ReservationDbContext>().SingleInstance();

            //register controllers
            builder.RegisterControllers(
                typeof(HomeController).Assembly,
                typeof(AdminLogic.Controllers.HomeController).Assembly,
                typeof(WechatAPI.Controllers.HomeController).Assembly);

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // set to my own resolver
            WeihanLi.Common.DependencyResolver.SetDependencyResolver(
                new WeihanLi.Common.AutofacDependencyResolver(container));

            #endregion DependenceInjection

            // redis config
            RedisManager.AddRedisConfig(option =>
            {
                option.CachePrefix = "ActivityReservation";
                option.ChannelPrefix = "ActivityReservation";
            });

            //register system settings
            SystemSettingsConfig.RegisterSystemSettings();
        }
    }
}
