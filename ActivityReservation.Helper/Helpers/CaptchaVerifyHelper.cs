using System;
using ActivityReservation.Common;
using Newtonsoft.Json;

namespace ActivityReservation.Helpers
{
    public class CaptchaVerifyHelper
    {
        private readonly GoogleRecaptchaHelper _googleRecaptchaHelper;
        private readonly TencentCaptchaHelper _tencentCaptchaHelper;

        public CaptchaVerifyHelper(GoogleRecaptchaHelper googleRecaptchaHelper,
            TencentCaptchaHelper tencentCaptchaHelper)
        {
            _googleRecaptchaHelper = googleRecaptchaHelper;
            _tencentCaptchaHelper = tencentCaptchaHelper;
        }

        public async System.Threading.Tasks.Task<bool> ValidateVerifyCodeAsync(string recaptchaType, string recaptcha)
        {
            if (recaptchaType.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (recaptchaType.Equals("Google", StringComparison.OrdinalIgnoreCase))
            {
                return await _googleRecaptchaHelper.IsValidRequestAsync(recaptcha);
            }
            if (recaptchaType.Equals("Tencent", StringComparison.OrdinalIgnoreCase))
            {
                return await _tencentCaptchaHelper.IsValidRequestAsync(JsonConvert.DeserializeObject<TencentCaptchaRequest>(recaptcha));
            }

            return false;
        }
    }
}
