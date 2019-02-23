using Tencent;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Logging;

namespace ActivityReservation.WechatAPI.Helper
{
    public class WechatSecurityHelper
    {
        private static readonly WXBizMsgCrypt Wxcpt =
            new WXBizMsgCrypt(WeChatConsts.Token, WeChatConsts.AESKey, WeChatConsts.AppId);

        private static readonly ILogHelperLogger Logger = LogHelper.GetLogger(typeof(WechatSecurityHelper));

        private readonly string _signature, _timestamp, _nonce;

        public WechatSecurityHelper(string signature, string timestamp, string nonce)
        {
            _signature = signature;
            _timestamp = timestamp;
            _nonce = nonce;
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
                Logger.Error("微信消息加密失败,result:" + result);
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
                Logger.Error("消息解密失败,result:" + result);
            }
            return decryptMsg;
        }
    }
}
