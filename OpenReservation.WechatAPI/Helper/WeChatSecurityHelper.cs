using Microsoft.Extensions.Logging;
using Tencent;

namespace OpenReservation.WechatAPI.Helper;

public class WechatSecurityHelper(string signature, string timestamp, string nonce, ILogger logger)
{
    private static readonly WXBizMsgCrypt Wxcpt =
        new(MpWeChatConsts.Token, MpWeChatConsts.AESKey, MpWeChatConsts.AppId);

    /// <summary>
    /// 加密消息
    /// </summary>
    /// <param name="msg">要加密的消息</param>
    /// <returns>加密后的消息</returns>
    public string EncryptMsg(string msg)
    {
        var encryptMsg = "";
        var result = Wxcpt.EncryptMsg(msg, timestamp, nonce, ref encryptMsg);
        if (result != 0)
        {
            logger.Error("微信消息加密失败,result:" + result);
        }
        return encryptMsg;
    }

    /// <summary>
    /// 解密消息
    /// </summary>
    /// <param name="msg">消息体</param>
    /// <returns>明文消息</returns>
    public string DecryptMsg(string msg)
    {
        var decryptMsg = "";
        var result = Wxcpt.DecryptMsg(signature, timestamp, nonce, msg, ref decryptMsg);
        if (result != 0)
        {
            logger.Error("消息解密失败,result:" + result);
        }
        return decryptMsg;
    }
}