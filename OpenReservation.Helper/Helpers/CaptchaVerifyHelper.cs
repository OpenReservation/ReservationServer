using System;
using Microsoft.AspNetCore.Http;
using OpenReservation.Common;
using WeihanLi.Extensions;
using WeihanLi.Web.Extensions;

namespace OpenReservation.Helpers;

public class CaptchaVerifyHelper(
    GoogleRecaptchaHelper googleRecaptchaHelper,
    TencentCaptchaHelper tencentCaptchaHelper,
    IHttpContextAccessor httpContextAccessor)
{
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
            return await googleRecaptchaHelper.IsValidRequestAsync(captchaInfo);
        }
        if (captchaType.Equals("Tencent", StringComparison.OrdinalIgnoreCase))
        {
            var request = captchaInfo.JsonToObject<TencentCaptchaRequest>();
            if (request.UserIP.IsNullOrWhiteSpace())
            {
                request.UserIP = httpContextAccessor.HttpContext.GetUserIP();
            }
            return await tencentCaptchaHelper.IsValidRequestAsync(request);
        }
        return false;
    }
}