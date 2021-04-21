using System;
using ActivityReservation.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WeihanLi.Common;
using WeihanLi.Extensions;
using WeihanLi.Web.Extensions;

namespace ActivityReservation.Helpers
{
    public class CaptchaVerifyHelper
    {
        private readonly TencentCaptchaHelper _tencentCaptchaHelper;

        public CaptchaVerifyHelper(TencentCaptchaHelper tencentCaptchaHelper)
        {
            _tencentCaptchaHelper = tencentCaptchaHelper;
        }

        public async System.Threading.Tasks.Task<bool> ValidateVerifyCodeAsync(string captchaType, string captchaInfo)
        {
            if (captchaType.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (captchaType.Equals("Tencent", StringComparison.OrdinalIgnoreCase))
            {
                var request = captchaInfo.JsonToObject<TencentCaptchaRequest>();
                if (request.UserIP.IsNullOrWhiteSpace())
                {
                    request.UserIP = DependencyResolver.Current.GetRequiredService<IHttpContextAccessor>()
                        .HttpContext.GetUserIP();
                }
                return await _tencentCaptchaHelper.IsValidRequestAsync(request);
            }

            return false;
        }
    }
}
