using ActivityReservation.WechatAPI.Helper;
using System;
using System.IO;
using System.Web.Mvc;

namespace ActivityReservation.WechatAPI.Controllers
{
    [Filters.WechatRequestValid]
    public class HomeController : Controller
    {
        /// <summary>
        /// 日志助手
        /// </summary>
        private static Common.LogHelper logger = new Common.LogHelper(typeof(HomeController));

        [HttpGet]
        [ActionName("Index")]
        public void Get(Model.WechatMsgRequestModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //GET微信验证，获取 echostr 参数并返回
                    string echoStr = HttpContext.Request.QueryString["echostr"];
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
        public void Post(Model.WechatMsgRequestModel model)
        {
            //从请求的数据流中获取请求信息
            using (Stream stream = HttpContext.Request.InputStream)
            {
                //创建 byte数组以接受从流中获取到的消息
                byte[] postBytes = new byte[stream.Length];
                //将POST请求中的数据流读入 准备好的 byte数组中
                stream.Read(postBytes, 0, (int)stream.Length);
                //从数据流中获取到字符串
                string postString = System.Text.Encoding.UTF8.GetString(postBytes);
                if(String.IsNullOrEmpty(postString))
                {
                    return;
                }
                logger.Debug("微信服务器消息："+postString);
                //
                model.Signature = Request.QueryString["msg_signature"];
                //处理响应
                WechatContext context = new WechatContext(model, postString);
                string responseContent = context.GetResponse();
                //设置输出编码
                HttpContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                //输出响应文本
                HttpContext.Response.Write(responseContent);
                //截止输出流
                HttpContext.Response.End();
            }
        }
    }
}