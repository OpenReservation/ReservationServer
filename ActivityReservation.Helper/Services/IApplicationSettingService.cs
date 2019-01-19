using System.Collections.Generic;

namespace ActivityReservation.Services
{
    public interface IApplicationSettingService
    {
        string GetSettingValue(string settingKey);

        string SetSettingValue(string settingKey, string settingValue);

        int AddSettings(Dictionary<string, string> dictionary);
    }
}
