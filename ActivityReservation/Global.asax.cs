using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Common;
using WeihanLi.Common.Helpers;

namespace ActivityReservation
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

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