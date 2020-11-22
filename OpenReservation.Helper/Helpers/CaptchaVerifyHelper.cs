using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenReservation.Common;
using WeihanLi.Common;
using WeihanLi.Extensions;
using WeihanLi.Web.Extensions;

namespace OpenReservation.Helpers
{
    public class CaptchaVerifyHelper
    {
        private readonly GoogleRecaptchaHelper _googleRecaptchaHelper;
        private readonly TencentCaptchaHelper _tencentCaptchaHelper;
        private readonly IHttpContextAccessor _contextAccessor;

        public CaptchaVerifyHelper(GoogleRecaptchaHelper googleRecaptchaHelper, TencentCaptchaHelper tencentCaptchaHelper, IHttpContextAccessor httpContextAccessor)
        {
            _googleRecaptchaHelper = googleRecaptchaHelper;
            _tencentCaptchaHelper = tencentCaptchaHelper;
            _contextAccessor = httpContextAccessor;
        }

        public async System.Threading.Tasks.Task<bool> ValidateVerifyCodeAsync(string captchaType, string captchaInfo)
        {
            if (string.IsNullOrWhiteSpace(captchaType))
            {
                captchaType = "Tencent";
            }
            if (captchaType.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (string.IsNullOrWhiteSpace(captchaInfo))
            {
                return false;
            }
            if (captchaType.Equals("Google", StringComparison.OrdinalIgnoreCase))
            {
                return await _googleRecaptchaHelper.IsValidRequestAsync(captchaInfo);
            }
            if (captchaType.Equals("Tencent", StringComparison.OrdinalIgnoreCase))
            {
                var request = captchaInfo.JsonToObject<TencentCaptchaRequest>();
                if (request.UserIP.IsNullOrWhiteSpace())
                {
                    request.UserIP = _contextAccessor.HttpContext.GetUserIP();
                }
                return await _tencentCaptchaHelper.IsValidRequestAsync(request);
            }
            return false;
        }
    }
}
