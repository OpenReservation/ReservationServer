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
        private readonly ILogger _logger;

        public CaptchaVerifyHelper(GoogleRecaptchaHelper googleRecaptchaHelper, TencentCaptchaHelper tencentCaptchaHelper, ILogger<CaptchaVerifyHelper> logger)
        {
            _googleRecaptchaHelper = googleRecaptchaHelper;
            _tencentCaptchaHelper = tencentCaptchaHelper;
            _logger = logger;
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
            if (string.IsNullOrEmpty(captchaInfo))
            {
                return false;
            }
            if (captchaType.Equals("Google", StringComparison.OrdinalIgnoreCase))
            {
                return await _googleRecaptchaHelper.IsValidRequestAsync(captchaInfo);
            }
            if (captchaType.Equals("Tencent", StringComparison.OrdinalIgnoreCase))
            {
                if (captchaInfo.IsNotNullOrWhiteSpace())
                {
                    try
                    {
                        var request = captchaInfo.JsonToObject<TencentCaptchaRequest>();
                        if (request.UserIP.IsNullOrWhiteSpace())
                        {
                            request.UserIP = DependencyResolver.Current.GetRequiredService<IHttpContextAccessor>()
                                .HttpContext.GetUserIP();
                        }
                        return await _tencentCaptchaHelper.IsValidRequestAsync(request);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "腾讯验证码信息异常");
                    }
                }
            }

            return false;
        }
    }
}
