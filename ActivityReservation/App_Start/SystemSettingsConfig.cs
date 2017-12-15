using System.Web;
using System.Web.Mvc;
using Business;

namespace ActivityReservation
{
    public class SystemSettingsConfig
    {
        public static void RegisterSystemSettings()
        {
            var settings = DependencyResolver.Current.GetService<IBLLSystemSettings>().GetAll();
            foreach (var item in settings)
            {
                HttpContext.Current.Application[item.SettingName] = item.SettingValue;
            }
        }
    }
}