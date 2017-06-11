using Common;

namespace ActivityReservation.WechatAPI.Helper
{
    public class WechatSecurityHelper
    {

        private static Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(WeChatConsts.Token, WeChatConsts.AESKey, WeChatConsts.AppId);
        private readonly string signature, timestamp, nonce;
        private static LogHelper logger = new LogHelper(typeof(WechatSecurityHelper));

        public WechatSecurityHelper(string signature , string timestamp , string nonce)
        {
            this.signature = signature;
            this.timestamp = timestamp;
            this.nonce = nonce;
        }

        /// <summary>
        /// 加密消息
        /// </summary>
        /// <param name="msg">要加密的消息</param>
        /// <returns>加密后的消息</returns>
        public string EncryptMsg(string msg)
        {
            string encryptMsg = "";
            int result = wxcpt.EncryptMsg(msg , timestamp , nonce , ref encryptMsg);
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
            string decryptMsg = "";
            int result = wxcpt.DecryptMsg(signature , timestamp , nonce , msg , ref decryptMsg);
            if (result != 0)
            {
                logger.Error("消息解密失败,result:" + result);
            }
            return decryptMsg;
        }
    }
}