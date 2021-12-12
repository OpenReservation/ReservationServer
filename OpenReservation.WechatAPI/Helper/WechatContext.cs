using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenReservation.WechatAPI.Model;
using WeihanLi.Common;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Logging;
using WeihanLi.Extensions;

namespace OpenReservation.WechatAPI.Helper;

internal class WeChatContext
{
    private readonly WechatSecurityHelper _securityHelper;
    private readonly string _requestMessage;
    private readonly ILogger _logger;

    public WeChatContext(WechatMsgRequestModel request, ILogger logger)
    {
        _securityHelper = new WechatSecurityHelper(request.Msg_Signature, request.Timestamp, request.Nonce, logger);
        _requestMessage = request.RequestContent;
        _logger = logger;
    }

    public async Task<string> GetResponseAsync()
    {
        var requestMessage = _securityHelper.DecryptMsg(_requestMessage);
        var responseMessage = await new MpWechatMsgHandler(_logger)
            .ReturnMessageAsync(requestMessage);
        _logger.Debug($"request:{requestMessage}, response:{responseMessage}");
        if (responseMessage.IsNotNullOrEmpty())
        {
            return _securityHelper.EncryptMsg(responseMessage);
        }
        return string.Empty;
    }
}