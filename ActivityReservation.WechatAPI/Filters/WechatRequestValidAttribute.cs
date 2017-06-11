using System;
using System.Text;
using System.Web.Mvc;

namespace ActivityReservation.WechatAPI.Filters
{
    public class WechatRequestValidAttribute : ActionFilterAttribute
    {
        private static Common.LogHelper logger = new Common.LogHelper(typeof(WechatRequestValidAttribute));
        Model.WechatMsgRequestModel model = new Model.WechatMsgRequestModel();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            model.Nonce = filterContext.HttpContext.Request.QueryString["nonce"];
            model.Signature = filterContext.HttpContext.Request.QueryString["signature"];
            model.Timestamp = filterContext.HttpContext.Request.QueryString["timestamp"];
            model.Msg_Signature = filterContext.HttpContext.Request.QueryString["msg_signature"];
            if(filterContext.HttpContext.Request.HttpMethod.ToUpper() == "POST")
            {
                var request = filterContext.HttpContext.Request;
                using (var reader = new System.IO.StreamReader(request.InputStream))
                {
                    model.RequestContent = reader.ReadToEnd();
                }
                request.ContentType = model.RequestContent;
            }
            logger.Debug("微信请求信息,"+Newtonsoft.Json.JsonConvert.SerializeObject(model)+","+Newtonsoft.Json.JsonConvert.SerializeObject(filterContext.HttpContext.Request.QueryString));
            //验证
            if (!CheckSignature(model))
            {
                logger.Error("微信请求签名验证不通过");
                ContentResult result = new ContentResult()
                {
                    Content = "微信请求验证失败" ,
                    ContentEncoding = Encoding.UTF8 ,
                    ContentType = "text/html"
                };
                filterContext.Result = result;
            }
            else
            {
                logger.Debug("微信请求签名验证通过");
                base.OnActionExecuting(filterContext);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        private bool CheckSignature(Model.WechatMsgRequestModel model)
        {
            string signature, timestamp, nonce, tempStr;
            //获取请求来的参数
            signature = model.Signature;
            timestamp = model.Timestamp;
            nonce = model.Nonce;
            //创建数组，将 Token, timestamp, nonce 三个参数加入数组
            string[] array = { Helper.WeChatConsts.Token , timestamp , nonce };
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