using Microsoft.Extensions.Logging;
using Tencent;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Logging;

namespace OpenReservation.WechatAPI.Helper
{
    public class WechatSecurityHelper
    {
        private static readonly WXBizMsgCrypt Wxcpt =
            new WXBizMsgCrypt(MpWeChatConsts.Token, MpWeChatConsts.AESKey, MpWeChatConsts.AppId);


        private readonly string _signature, _timestamp, _nonce;

        private readonly ILogger _logger;

        public WechatSecurityHelper(string signature, string timestamp, string nonce, ILogger logger)
        {
            _signature = signature;
            _timestamp = timestamp;
            _nonce = nonce;
            _logger = logger;
        }

        /// <summary>
        /// 加密消息
        /// </summary>
        /// <param name="msg">要加密的消息</param>
        /// <returns>加密后的消息</returns>
        public string EncryptMsg(string msg)
        {
            var encryptMsg = "";
            var result = Wxcpt.EncryptMsg(msg, _timestamp, _nonce, ref encryptMsg);
            if (result != 0)
            {
                _logger.Error("微信消息加密失败,result:" + result);
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
            var result = Wxcpt.DecryptMsg(_signature, _timestamp, _nonce, msg, ref decryptMsg);
            if (result != 0)
            {
                _logger.Error("消息解密失败,result:" + result);
            }
            return decryptMsg;
        }
    }
}
