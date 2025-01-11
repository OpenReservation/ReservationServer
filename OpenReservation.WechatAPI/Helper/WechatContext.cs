using Microsoft.Extensions.Logging;
using OpenReservation.WechatAPI.Model;
using WeihanLi.Extensions;

namespace OpenReservation.WechatAPI.Helper;

internal class WeChatContext(WechatMsgRequestModel request, ILogger logger)
{
    private readonly WechatSecurityHelper _securityHelper = new(request.Msg_Signature, request.Timestamp, request.Nonce, logger);
    private readonly string _requestMessage = request.RequestContent;

    public async Task<string> GetResponseAsync()
    {
        var requestMessage = _securityHelper.DecryptMsg(_requestMessage);
        var responseMessage = await new MpWechatMsgHandler(logger)
            .ReturnMessageAsync(requestMessage);
        logger.Debug($"request:{requestMessage}, response:{responseMessage}");
        if (responseMessage.IsNotNullOrEmpty())
        {
            return _securityHelper.EncryptMsg(responseMessage);
        }
        return string.Empty;
    }
}