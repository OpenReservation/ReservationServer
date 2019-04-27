using ActivityReservation.WechatAPI.Model;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Logging;
using WeihanLi.Extensions;

namespace ActivityReservation.WechatAPI.Helper
{
    public class WechatContext
    {
        private static readonly ILogHelperLogger Logger = LogHelper.GetLogger(typeof(WechatContext));
        private readonly WechatSecurityHelper _securityHelper;
        private readonly string _requestMessage;

        public WechatContext()
        {
        }

        public WechatContext(WechatMsgRequestModel request)
        {
            Logger.Log(LogHelperLevel.Debug, "微信服务器消息：" + request.ToJson(), null);
            _securityHelper = new WechatSecurityHelper(request.Msg_Signature, request.Timestamp, request.Nonce);
            _requestMessage = _securityHelper.DecryptMsg(request.RequestContent);
            Logger.Log(LogHelperLevel.Debug, "收到微信消息：" + _requestMessage, null);
        }

        public string GetResponse()
        {
            var responseMessage = WechatMsgHandler.ReturnMessage(_requestMessage);
            if (responseMessage.IsNotNullOrEmpty())
            {
                Logger.Log(LogHelperLevel.Debug, "返回消息：" + responseMessage, null);
                return _securityHelper.EncryptMsg(responseMessage);
            }
            return string.Empty;
        }
    }
}
