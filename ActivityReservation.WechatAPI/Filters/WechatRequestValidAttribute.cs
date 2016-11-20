using System;
using System.Web.Mvc;

namespace ActivityReservation.WechatAPI.Filters
{
    public class WechatRequestValidAttribute : ActionFilterAttribute
    {
        private const string Token = "Reservation";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //参数适配
            Model.WechatMsgRequestModel model = new Model.WechatMsgRequestModel()
            {
                Nonce = filterContext.HttpContext.Request.QueryString["nonce"] ,
                MsgSignature = filterContext.HttpContext.Request.QueryString["msg_signature"] ,
                Timestamp = filterContext.HttpContext.Request.QueryString["timestamp"]
            };
            if (filterContext.HttpContext.Request.HttpMethod.ToUpperInvariant().Equals("GET"))
            {
                model.MsgSignature = filterContext.HttpContext.Request.QueryString["signature"];
            }
            //验证
            if (CheckSignature(model))
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                filterContext.HttpContext.Response.Write("微信请求验证失败");
                filterContext.HttpContext.Response.End();//停止处理结束响应
            }
        }

        private bool CheckSignature(Model.WechatMsgRequestModel model)
        {
            string signature, timestamp, nonce, tempStr;
            //获取请求来的参数
            signature = model.MsgSignature;
            timestamp = model.Timestamp;
            nonce = model.Nonce;
            //创建数组，将 Token, timestamp, nonce 三个参数加入数组
            string[] array = { Token , timestamp , nonce };
            //进行排序
            Array.Sort(array);
            //拼接为一个字符串
            tempStr = String.Join("" , array);
            //对字符串进行 SHA1加密
            tempStr = Common.SecurityHelper.SHA1_Encrypt(tempStr);
            //判断signature 是否正确
            if (tempStr.Equals(signature.ToUpperInvariant()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}