using System;
using System.Text;
using System.Web.Mvc;
using ActivityReservation.WechatAPI.Helper;
using ActivityReservation.WechatAPI.Model;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.WechatAPI.Filters
{
    public class WechatRequestValidAttribute : FilterAttribute, IAuthorizationFilter
    {
        private static readonly LogHelper Logger = new LogHelper(typeof(WechatRequestValidAttribute));

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var model = new WechatMsgRequestModel
            {
                Nonce = filterContext.HttpContext.Request.QueryString["nonce"],
                Signature = filterContext.HttpContext.Request.QueryString["signature"],
                Timestamp = filterContext.HttpContext.Request.QueryString["timestamp"],
                Msg_Signature = filterContext.HttpContext.Request.QueryString["msg_signature"]
            };
            //验证
            if (!CheckSignature(model))
            {
                Logger.Error("微信请求签名验证不通过");
                filterContext.Result = new ContentResult
                {
                    Content = "微信请求验证失败",
                    ContentEncoding = Encoding.UTF8,
                    ContentType = "text/html"
                };
            }
        }

        private bool CheckSignature(WechatMsgRequestModel model)
        {
            //获取请求来的参数
            var signature = model.Signature;
            var timestamp = model.Timestamp;
            var nonce = model.Nonce;
            //创建数组，将 Token, timestamp, nonce 三个参数加入数组
            string[] array = { WeChatConsts.Token, timestamp, nonce };
            //进行排序
            Array.Sort(array);
            //拼接为一个字符串
            var tempStr = string.Join("", array);
            //对字符串进行 SHA1加密
            tempStr = SecurityHelper.SHA1_Encrypt(tempStr);
            //判断signature 是否正确
            return tempStr.Equals(signature.ToUpper());
        }
    }
}
