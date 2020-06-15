using System;
using System.Text;
using System.Threading.Tasks;
using OpenReservation.Common;
using OpenReservation.WechatAPI.Helper;
using OpenReservation.WechatAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeihanLi.Extensions;
using WeihanLi.Redis;

namespace OpenReservation.WechatAPI.Controllers
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public class WeChatAppController : WeChatBaseController
    {
        public WeChatAppController(ILogger<WeChatAppController> logger) : base(logger)
        {
        }

        [HttpGet]
        [ActionName("Index")]
        public async Task Get(string echoStr)
        {
            if (!string.IsNullOrEmpty(echoStr))
            {
                await Response.WriteAsync(echoStr, HttpContext.RequestAborted);
            }
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> Post()
        {
            var body = await Request.Body.ReadToEndAsync();
            Logger.LogInformation($"received msg: {body}");
            var model = body.JsonToObject<WeChatTextMsgModel>();
            if (!string.IsNullOrWhiteSpace(model?.FromUserName))
            {
                try
                {
                    if (await RedisManager.GetFirewallClient($"wxAppMsg:{model.MsgId}", TimeSpan.FromMinutes(2)).HitAsync())
                    {
                        Logger.LogInformation($"msgModel: {model.ToJson()}");
                        var wechatHelper = HttpContext.RequestServices.GetRequiredService<WeChatHelper>();

                        switch (model.MsgType)
                        {
                            case "text":
                                var chatbotHelper = HttpContext.RequestServices.GetRequiredService<ChatBotHelper>();
                                var reply = await chatbotHelper.GetBotReplyAsync(model.Content);
                                if (reply.IsNotNullOrEmpty())
                                {
                                    Logger.LogInformation($"bot reply:{reply}");
                                    //
                                    await wechatHelper.SendWechatMsg(new
                                    {
                                        touser = model.FromUserName,
                                        msgtype = "text",
                                        text = new
                                        {
                                            content = reply
                                        }
                                    }, WxAppConsts.AppId, WxAppConsts.AppSecret).ConfigureAwait(false);
                                }
                                break;

                            case "image":
                                var imgMsg = body.JsonToObject<WeChatImageMsgModel>();
                                // 返回原图
                                await wechatHelper.SendWechatMsg(new
                                {
                                    touser = model.FromUserName,
                                    msgtype = "text",
                                    text = new
                                    {
                                        content = "抱歉，人家还没学会自动识别图片呢"
                                    }
                                }, WxAppConsts.AppId, WxAppConsts.AppSecret);
                                break;

                            case "event":
                                //
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e, "微信小程序客服消息处理发生");
                }
            }

            return Content("success", "text/plain", Encoding.UTF8);
        }
    }
}
