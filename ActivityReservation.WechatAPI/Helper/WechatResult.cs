using Senparc.Weixin.MessageHandlers;
using System.Text;
using System.Web.Mvc;

namespace ActivityReservation.WechatAPI.Helper
{
    public class WechatResult : ContentResult
    {
        protected IMessageHandlerDocument _messageHandlerDocument;

        public WechatResult(IMessageHandlerDocument messageHandlerDocument)
        {
            _messageHandlerDocument = messageHandlerDocument;
        }

        public WechatResult(string content)
        {
            //_content = content;
            base.Content = content;
        }

        new public string Content
        {
            get
            {
                if (base.Content != null)
                {
                    return base.Content;
                }
                if (_messageHandlerDocument != null)
                {
                    if (_messageHandlerDocument.TextResponseMessage != null)
                    {
                        return _messageHandlerDocument.TextResponseMessage.Replace("\r\n", "\n");
                    }
                }
                return null;
            }
            set { base.Content = value; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var content = this.Content;

            if (content == null)
            {
                //使用IMessageHandler输出
                if (_messageHandlerDocument == null)
                {
                    throw new Senparc.Weixin.Exceptions.WeixinException("执行WeixinResult时提供的MessageHandler不能为Null！", null);
                }
                var finalResponseDocument = _messageHandlerDocument.FinalResponseDocument;

                if (finalResponseDocument == null)
                {
                    //throw new Senparc.Weixin.MP.WeixinException("FinalResponseDocument不能为Null！", null);
                }
                else
                {
                    content = finalResponseDocument.ToString();
                }
            }
            context.HttpContext.Response.ClearContent();
            context.HttpContext.Response.ContentType = "text/xml";
            content = (content ?? "").Replace("\r\n", "\n");

            var bytes = Encoding.UTF8.GetBytes(content);
            context.HttpContext.Response.OutputStream.Write(bytes, 0, bytes.Length);
        }
    }
}