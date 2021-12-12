using System.Collections.Generic;
using WeihanLi.Redis;

namespace OpenReservation.Services;

public class ApplicationSettingInRedisService : IApplicationSettingService
{
    private readonly IHashClient _hashClient;
    private const string ApplicationSettingKey = "GlobalApplicationSettings";

    public ApplicationSettingInRedisService(IHashClient hashClient)
    {
        _hashClient = hashClient;
    }

    public string GetSettingValue(string settingKey)
    {
        return _hashClient.Get(ApplicationSettingKey, settingKey);
    }

    public string SetSettingValue(string settingKey, string settingValue)
    {
        _hashClient.Set(ApplicationSettingKey, settingKey, settingValue);
        return settingValue;
    }

    public int AddSettings(Dictionary<string, string> dictionary)
    {
        if (dictionary != null && dictionary.Count > 0)
        {
            _hashClient.Set(ApplicationSettingKey, dictionary);
            return dictionary.Count;
        }
        return 0;
    }
}