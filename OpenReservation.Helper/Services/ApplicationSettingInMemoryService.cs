using System.Collections.Concurrent;

namespace OpenReservation.Services;

public sealed class ApplicationSettingInMemoryService : IApplicationSettingService
{
    private readonly ConcurrentDictionary<string, string> _settingDictionary = new();

    public int AddSettings(Dictionary<string, string> dictionary)
    {
        if (dictionary is { Count: > 0 })
        {
            foreach (var item in dictionary)
            {
                _settingDictionary[item.Key] = item.Value;
            }
            return dictionary.Count;
        }
        return 0;
    }

    public string GetSettingValue(string settingKey)
    {
        _settingDictionary.TryGetValue(settingKey, out var val);
        return val;
    }

    public string SetSettingValue(string settingKey, string settingValue)
    {
        _settingDictionary[settingKey] = settingValue;
        return settingValue;
    }
}
