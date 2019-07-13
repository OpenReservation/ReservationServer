using System;
using System.Linq;
using ActivityReservation.WechatAPI.Helper;
using ActivityReservation.WechatAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Logging;

namespace ActivityReservation.WechatAPI.Filters
{
    public class WechatRequestValidAttribute : Attribute, IAuthorizationFilter
    {
        private static readonly ILogHelperLogger Logger = LogHelper.GetLogger(typeof(WechatRequestValidAttribute));

        private static bool CheckSignature(WechatMsgRequestModel model)
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
            tempStr = SecurityHelper.SHA1(tempStr);
            //判断signature 是否正确
            return tempStr.Equals(signature?.ToUpper());
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var model = new WechatMsgRequestModel
            {
                Nonce = filterContext.HttpContext.Request.Query["nonce"].FirstOrDefault(),
                Signature = filterContext.HttpContext.Request.Query["signature"].FirstOrDefault(),
                Timestamp = filterContext.HttpContext.Request.Query["timestamp"].FirstOrDefault(),
                Msg_Signature = filterContext.HttpContext.Request.Query["msg_signature"].FirstOrDefault()
            };
            //验证
            if (!CheckSignature(model))
            {
                Logger.Error("微信请求签名验证不通过");
                filterContext.Result = new ContentResult
                {
                    Content = "微信请求验证失败",
                    StatusCode = 401,
                    ContentType = "text/plain;charset=utf-8",
                };
            }
        }
    }
}
