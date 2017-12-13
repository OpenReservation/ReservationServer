using System;
using System.Collections.Generic;
using System.Linq;
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
            foreach (Models.SystemSettings item in settings)
            {
                HttpContext.Current.Application[item.SettingName] = item.SettingValue;
            }
        }
    }
}