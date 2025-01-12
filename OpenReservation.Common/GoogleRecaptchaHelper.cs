using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeihanLi.Extensions;

namespace OpenReservation.Common;

public class GoogleRecaptchaOptions
{
    public string SiteKey { get; set; }
    public string Secret { get; set; }
}

public class GoogleRecaptchaHelper(
    IOptions<GoogleRecaptchaOptions> option,
    ILogger<GoogleRecaptchaHelper> logger,
    HttpClient httpClient)
{
    /// <summary>
    /// GoogleRecaptchaVerifyUrl
    /// </summary>
    private const string GoogleRecaptchaVerifyUrl = "https://www.google.com/recaptcha/api/siteverify";

    private readonly GoogleRecaptchaOptions _recaptchaOptions = option.Value;
    private readonly ILogger _logger = logger;

    public async Task<bool> IsValidRequestAsync(string recaptchaResponse)
    {
        var response = await httpClient.PostAsync(GoogleRecaptchaVerifyUrl, new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            {"response", recaptchaResponse},
            {"secret", _recaptchaOptions.Secret }
        }));
        var responseText = await response.Content.ReadAsStringAsync();
        if (responseText.IsNotNullOrEmpty())
        {
            var result = responseText.JsonToObject<GoogleRecaptchaVerifyResponse>();
            if (result.Success)
            {
                return true;
            }
            _logger.Warn($"GoogleRecaptchaVerifyFail, response:{response.ToJson()}");
        }
        return false;
    }

    private class GoogleRecaptchaVerifyResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public string ChallengeTs { get; set; }

        [JsonProperty("error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}