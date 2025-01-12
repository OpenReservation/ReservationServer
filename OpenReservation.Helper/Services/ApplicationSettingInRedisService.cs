using System.Collections.Generic;
using WeihanLi.Redis;

namespace OpenReservation.Services;

public class ApplicationSettingInRedisService(IHashClient hashClient) : IApplicationSettingService
{
    private const string ApplicationSettingKey = "GlobalApplicationSettings";

    public string GetSettingValue(string settingKey)
    {
        return hashClient.Get(ApplicationSettingKey, settingKey);
    }

    public string SetSettingValue(string settingKey, string settingValue)
    {
        hashClient.Set(ApplicationSettingKey, settingKey, settingValue);
        return settingValue;
    }

    public int AddSettings(Dictionary<string, string> dictionary)
    {
        if (dictionary is { Count: > 0 })
        {
            hashClient.Set(ApplicationSettingKey, dictionary);
            return dictionary.Count;
        }
        return 0;
    }
}
