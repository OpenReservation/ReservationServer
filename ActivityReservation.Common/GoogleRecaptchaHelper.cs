using System.Collections.Generic;
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

        public GoogleRecaptchaHelper(IOptions<GoogleRecaptchaOptions> option, ILogger<GoogleRecaptchaHelper> logger)
        {
            _recaptchaOptions = option.Value;
            _logger = logger;
        }

        public bool IsValidRequest(string recaptchaResponse)
        {
            var response = HttpHelper.HttpPost(GoogleRecaptchaVerifyUrl, new Dictionary<string, string>
            {
                {"response", recaptchaResponse},
                {"secret", GoogleRecaptchaSecret }
            }).JsonToType<GoogleRecaptchaVerifyResponse>();
            if (response.Success)
            {
                return true;
            }
            _logger.Error($"GoogleRecaptchaVerifyFail, response:{recaptchaResponse},error codes:{string.Join(",", response.ErrorCodes ?? new string[0])}");
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
