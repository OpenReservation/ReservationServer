using ActivityReservation.WechatAPI.Model;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Logging;
using WeihanLi.Extensions;

namespace ActivityReservation.WechatAPI.Helper
{
    public class WechatContext
    {
        private readonly WechatSecurityHelper _securityHelper;
        private readonly string _requestMessage;
        private static readonly ILogHelperLogger Logger = LogHelper.GetLogger<WechatContext>();

        public WechatContext()
        {
        }

        public WechatContext(WechatMsgRequestModel request)
        {
            _securityHelper = new WechatSecurityHelper(request.Msg_Signature, request.Timestamp, request.Nonce);
            _requestMessage = request.RequestContent;
        }

        public async System.Threading.Tasks.Task<string> GetResponseAsync()
        {
            var requestMessage = _securityHelper.DecryptMsg(_requestMessage);
            var responseMessage = await WechatMsgHandler.ReturnMessageAsync(requestMessage);
            if (responseMessage.IsNotNullOrEmpty())
            {
                Logger.Debug($"request:{requestMessage}, response:{responseMessage}");
                return _securityHelper.EncryptMsg(responseMessage);
            }
            return string.Empty;
        }
    }
}
