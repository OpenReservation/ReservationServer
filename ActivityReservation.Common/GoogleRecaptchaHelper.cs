using System.Collections.Generic;
using Newtonsoft.Json;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Log;

namespace ActivityReservation.Common
{
    public static class GoogleRecaptchaHelper
    {
        /// <summary>
        /// GoogleRecaptchaVerifyUrl
        /// </summary>
        private const string GoogleRecaptchaVerifyUrl = "https://www.google.com/recaptcha/api/siteverify";

        private static readonly string GoogleRecaptchaSecret = ConfigurationHelper.AppSetting("GoogleRecaptchaSecret");

        private static readonly ILogHelper Logger = LogHelper.GetLogHelper(typeof(GoogleRecaptchaHelper));

        public static bool IsValidRequest(string recaptchaResponse)
        {
            var response = ConvertHelper.JsonToObject<GoogleRecaptchaVerifyResponse>(HttpHelper.HttpPost(GoogleRecaptchaVerifyUrl, new Dictionary<string, string>
            {
                {"response", recaptchaResponse},
                {"secret", GoogleRecaptchaSecret }
            }));
            if (response.Success)
            {
                return true;
            }
            Logger.Error($"GoogleRecaptchaVerifyFail, response:{recaptchaResponse},error codes:{string.Join(",", response.ErrorCodes ?? new string[0])}");
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
