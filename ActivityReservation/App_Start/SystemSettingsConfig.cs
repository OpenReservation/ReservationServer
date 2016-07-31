using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivityReservation
{
    public class SystemSettingsConfig
    {
        public static void RegisterSystemSettings()
        {
            List<Models.SystemSettings> settings = new Business.BLLSystemSettings().GetAll();
            foreach (Models.SystemSettings item in settings)
            {
                HttpContext.Current.Application[item.SettingName] = item.SettingValue;
            }
        }
    }
}