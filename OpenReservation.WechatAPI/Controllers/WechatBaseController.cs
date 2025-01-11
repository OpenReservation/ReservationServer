using OpenReservation.WechatAPI.Filters;
using OpenReservation.WechatAPI.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.Extensions;
using OpenReservation.WechatAPI.Model;

namespace OpenReservation.WechatAPI.Controllers;

[Area("WeChat")]
[WechatRequestValid]
public class WeChatBaseController(ILogger logger) : Controller
{
    /// <summary>
    /// logger
    /// </summary>
    protected readonly ILogger Logger = logger;

    internal async Task<ContentResult> WechatAsync(WechatMsgRequestModel request)
    {
        var wechatContext = new WeChatContext(request, Logger);
        var response = await wechatContext.GetResponseAsync();
        if (response.IsNullOrEmpty())
        {
            return Content("");
        }
        return new WechatResult(response);
    }
}
