using ActivityReservation.WechatAPI.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ActivityReservation.WechatAPI.Controllers
{
    public class HomeController:Controller
    {
        /// <summary>
        /// 日志助手
        /// </summary>
        private static Common.LogHelper logger = new Common.LogHelper(typeof(HomeController));

        [Filters.WeChatRequestValid]
        public void Valid(Model.WeChatMsgRequestModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //判断是否是POST请求
                    if (HttpContext.Request.HttpMethod.ToUpperInvariant().Equals("POST"))
                    {
                        //从请求的数据流中获取请求信息
                        using (Stream stream = HttpContext.Request.InputStream)
                        {
                            //创建 byte数组以接受从流中获取到的消息
                            byte[] postBytes = new byte[stream.Length];
                            //将POST请求中的数据流读入 准备好的 byte数组中
                            stream.Read(postBytes , 0 , (int) stream.Length);
                            //从数据流中获取到字符串
                            string postString = System.Text.Encoding.UTF8.GetString(postBytes);
                            //处理响应
                            Handle(postString , model);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("发生异常,异常信息：" + ex.Message + ex.StackTrace);
                }
            }
        }

        private void Handle(string postStr , Model.WeChatMsgRequestModel model)
        {
            WechatMsgHelper msgHelper = new WechatMsgHelper();
            //消息加密、解密
            WechatSecurityHelper securityHelper = new WechatSecurityHelper(model.msg_signature , model.timestamp , model.nonce);
            string responseContent = msgHelper.ReturnMessage(securityHelper.DecryptMsg(postStr));
            responseContent = securityHelper.EncryptMsg(responseContent);
            //设置输出编码
            HttpContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //输出响应文本
            HttpContext.Response.Write(responseContent);
            //截止输出流
            HttpContext.Response.End();
        }
    }
}
