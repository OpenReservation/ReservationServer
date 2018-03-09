using System;
using System.IO;
using System.Web.Mvc;
using ActivityReservation.WechatAPI.Helper;
using ActivityReservation.WechatAPI.Model;

namespace ActivityReservation.WechatAPI.Controllers
{
    public class HomeController : WechatBaseController
    {
        [HttpGet]
        [ActionName("Index")]
        public void Get(WechatMsgRequestModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var echoStr = HttpContext.Request.QueryString["echostr"];
                    if (!string.IsNullOrEmpty(echoStr))
                    {
                        //将随机生成的 echostr 参数 原样输出
                        Response.Write(echoStr);
                        //截止输出流
                        Response.End();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Wechat GET 发生异常,异常信息：{ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="model">微信消息</param>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult PostAsync(WechatMsgRequestModel model)
        {
            if (model.RequestContent == null)
            {
                using (var reader = new StreamReader(Request.InputStream))
                {
                    Logger.Debug($"Request.InputStream Length:{Request.InputStream.Length}");
                    model.RequestContent = reader.ReadToEnd();
                    Logger.Debug($"RequestContent from Request.InputStream:{model.RequestContent}");
                }
            }
            if (string.IsNullOrEmpty(model.RequestContent))
            {
                return Content("RequestContent 为空");
            }
            var context = new WechatContext(model);
            return Wechat(context);
        }
    }
}
