using ActivityReservation.WechatAPI.Helper;
using ActivityReservation.WechatAPI.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

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
                    //GET微信验证，获取 echostr 参数并返回
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
                    logger.Error("发生异常,异常信息：" + ex.Message + ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="model">微信消息</param>
        [HttpPost]
        [ActionName("Index")]
        public async Task<ActionResult> PostAsync(WechatMsgRequestModel model)
        {
            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            //var postModel = new PostModel
            //{
            //    Nonce = model.Nonce,
            //    Timestamp = model.Timestamp,
            //    Signature = model.Signature,
            //    Msg_Signature = model.Msg_Signature,
            //    AppId = WeChatConsts.AppId,
            //    EncodingAESKey = WeChatConsts.AESKey,
            //    Token = WeChatConsts.Token,
            //};
            if (model.RequestContent == null)
            {
                using (var reader = new StreamReader(Request.InputStream))
                {
                    logger.Debug($"Request.InputStream Length:{Request.InputStream.Length}");
                    model.RequestContent = reader.ReadToEnd();
                    logger.Debug($"RequestContent from Request.InputStream:{model.RequestContent}");
                }
            }
            if (String.IsNullOrEmpty(model.RequestContent))
            {
                return Content("RequestContent 为空");
            }
            //var doc = System.Xml.Linq.XDocument.Parse(model.RequestContent);
            //logger.Debug("doc:" + doc.ToString());
            //var messageHandler = new WechatMsgHandler(doc, postModel);
            //#region 设置消息去重
            ///* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
            // * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage
            // */
            //messageHandler.OmitRepeatedMessage = true;//默认已经开启，此处仅作为演示，也可以设置为false在本次请求中停用此功能
            //#endregion
            //logger.Debug("收到微信消息：" + Common.ConverterHelper.ObjectToJson(messageHandler.RequestDocument));
            //messageHandler.Execute();
            //logger.Debug("返回的消息：" + Common.ConverterHelper.ObjectToJson(messageHandler.ResponseDocument));
            //return Wechat(messageHandler);
            var context = new WechatContext(model);
            return await Wechat(context);
        }
    }
}