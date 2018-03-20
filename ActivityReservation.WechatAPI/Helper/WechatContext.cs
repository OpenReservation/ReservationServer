using ActivityReservation.WechatAPI.Model;
using Newtonsoft.Json;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Log;
using WeihanLi.Extensions;

namespace ActivityReservation.WechatAPI.Helper
{
    public class WechatContext
    {
        private static readonly ILogHelper Logger = LogHelper.GetLogHelper(typeof(WechatContext));
        private readonly WechatSecurityHelper _securityHelper;
        private readonly string _requestMessage;

        public WechatContext()
        {
        }

        public WechatContext(WechatMsgRequestModel request)
        {
            Logger.Debug("微信服务器消息：" + JsonConvert.SerializeObject(request));
            _securityHelper = new WechatSecurityHelper(request.Msg_Signature, request.Timestamp, request.Nonce);
            _requestMessage = _securityHelper.DecryptMsg(request.RequestContent);
            Logger.Debug("收到微信消息：" + _requestMessage);
        }

        public string GetResponse()
        {
            var responseMessage = WechatMsgHandler.ReturnMessage(_requestMessage);
            if (responseMessage.IsNotNullOrEmpty())
            {
                Logger.Debug("返回消息：" + responseMessage);
                return _securityHelper.EncryptMsg(responseMessage);
            }
            return string.Empty;
        }
    }
}
