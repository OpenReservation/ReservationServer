using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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
            //register system settings
            RegisterSystemSettings();
            //log4net init
            Common.LogHelper.LogInit(Server.MapPath("log4net.config"));
        }

        public void RegisterSystemSettings()
        {
            List<Models.SystemSettings> settings = new Business.BLLSystemSettings().GetAll();
            foreach (Models.SystemSettings item in settings)
            {
                Application[item.SettingName] = item.SettingValue;
            }
        }
    }
}