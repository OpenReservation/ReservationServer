using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ActivityReservation.Controllers;
using Autofac;
using Autofac.Integration.Mvc;
using Common;
using WeihanLi.Common.Helpers;

namespace ActivityReservation
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();

            //register modules
            builder.RegisterAssemblyModules(BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray());

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

            //log4net init
            LogHelper.LogInit(Server.MapPath("log4net.config"), new ILogProvider[]
            {
                new ExceptionlessLogProvider(),
                new SentryLogProvider()
            });

            //register system settings
            SystemSettingsConfig.RegisterSystemSettings();
        }
    }
}