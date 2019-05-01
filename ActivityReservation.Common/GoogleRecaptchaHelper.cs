using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Logging;
using WeihanLi.Extensions;

namespace ActivityReservation.Common
{
    public class GoogleRecaptchaOptions
    {
        public string SiteKey { get; set; }
        public string Secret { get; set; }
    }

    public class GoogleRecaptchaHelper
    {
        /// <summary>
        /// GoogleRecaptchaVerifyUrl
        /// </summary>
        private const string GoogleRecaptchaVerifyUrl = "https://www.google.com/recaptcha/api/siteverify";

        private static readonly string GoogleRecaptchaSecret = ConfigurationHelper.AppSetting("GoogleRecaptchaSecret");
        private readonly GoogleRecaptchaOptions _recaptchaOptions;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public GoogleRecaptchaHelper(
            IOptions<GoogleRecaptchaOptions> option,
            ILogger<GoogleRecaptchaHelper> logger,
            HttpClient httpClient)
        {
            _recaptchaOptions = option.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public bool IsValidRequest(string recaptchaResponse)
        {
            var response =
            new HttpRequester(GoogleRecaptchaVerifyUrl, System.Net.Http.HttpMethod.Post)
            .WithFormParameters(new Dictionary<string, string>
            {
                    {"response", recaptchaResponse},
                    {"secret", GoogleRecaptchaSecret }
            })
            .ExecuteForJson<GoogleRecaptchaVerifyResponse>();
            if (response.Success)
            {
                return true;
            }
            _logger.Warn($"GoogleRecaptchaVerifyFail, response:{response.ToJson()}");
            return false;
        }

        public async Task<bool> IsValidRequestAsync(string recaptchaResponse)
        {
            var response = await _httpClient.PostAsync(GoogleRecaptchaVerifyUrl, new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                       {"response", recaptchaResponse},
                       {"secret", GoogleRecaptchaSecret }
                }));
            var responseText = await response.Content.ReadAsStringAsync();
            if (responseText.IsNotNullOrEmpty())
            {
                var result = responseText.JsonToType<GoogleRecaptchaVerifyResponse>();
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
}
