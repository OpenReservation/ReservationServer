using System;
using ActivityReservation.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WeihanLi.Common;
using WeihanLi.Extensions;

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

        public async System.Threading.Tasks.Task<bool> ValidateVerifyCodeAsync(string captchaType, string captchaInfo)
        {
            if (captchaType.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (captchaType.Equals("Google", StringComparison.OrdinalIgnoreCase))
            {
                return await _googleRecaptchaHelper.IsValidRequestAsync(captchaInfo);
            }
            if (captchaType.Equals("Tencent", StringComparison.OrdinalIgnoreCase))
            {
                var request = JsonConvert.DeserializeObject<TencentCaptchaRequest>(captchaInfo);
                if (request.UserIP.IsNullOrWhiteSpace())
                {
                    request.UserIP = DependencyResolver.Current.ResolveService<IHttpContextAccessor>().HttpContext.Connection.RemoteIpAddress.ToString();
                }
                return await _tencentCaptchaHelper.IsValidRequestAsync(request);
            }

            return false;
        }
    }
}
