using OpenReservation.WechatAPI.Model;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Logging;
using WeihanLi.Extensions;

namespace OpenReservation.WechatAPI.Helper
{
    internal class WeChatContext
    {
        private readonly WechatSecurityHelper _securityHelper;
        private readonly string _requestMessage;
        private static readonly ILogHelperLogger Logger = LogHelper.GetLogger<WeChatContext>();

        public WeChatContext()
        {
        }

        public WeChatContext(WechatMsgRequestModel request)
        {
            _securityHelper = new WechatSecurityHelper(request.Msg_Signature, request.Timestamp, request.Nonce);
            _requestMessage = request.RequestContent;
        }

        public async System.Threading.Tasks.Task<string> GetResponseAsync()
        {
            var requestMessage = _securityHelper.DecryptMsg(_requestMessage);
            var responseMessage = await MpWechatMsgHandler.ReturnMessageAsync(requestMessage);
            Logger.Debug($"request:{requestMessage}, response:{responseMessage}");
            if (responseMessage.IsNotNullOrEmpty())
            {
                return _securityHelper.EncryptMsg(responseMessage);
            }
            return string.Empty;
        }
    }
}
