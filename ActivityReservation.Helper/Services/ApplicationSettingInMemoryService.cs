using System.Collections.Generic;

namespace ActivityReservation.Services
{
    public class ApplicationSettingInMemoryService : IApplicationSettingService
    {
        private readonly Dictionary<string, string> _settingDictionary = new Dictionary<string, string>();

        public int AddSettings(Dictionary<string, string> dictionary)
        {
            if (dictionary != null && dictionary.Count > 0)
            {
                foreach (var item in dictionary)
                {
                    _settingDictionary[item.Key] = item.Value;
                }
            }
            return _settingDictionary.Count;
        }

        public string GetSettingValue(string settingKey)
        {
            _settingDictionary.TryGetValue(settingKey, out var val);
            return val;
        }

        public string SetSettingValue(string settingKey, string settingValue)
        {
            _settingDictionary[settingKey] = settingKey;
            return settingValue;
        }
    }
}
