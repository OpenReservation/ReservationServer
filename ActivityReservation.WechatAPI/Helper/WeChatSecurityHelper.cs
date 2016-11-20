using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace ActivityReservation.WechatAPI.Helper
{
    public class WechatSecurityHelper
    {
        /// <summary>
        /// 定义Token，与微信公共平台上的Token保持一致
        /// </summary>
        private const string Token = "Reservation";
        /// <summary>
        /// AppId 要与 微信公共平台 上的 AppId 保持一致
        /// </summary>
        private const string AppId = "wx7858bf8ff81c0235";
        /// <summary>
        /// 加密密钥
        /// </summary>
        private const string AESKey = "pvX2KZWRLQSkUAbvArgLSAxCwTtxgFWF3XOnJ9iEUMG";

        private static Tencent.WXBizMsgCrypt wxcpt = new Tencent.WXBizMsgCrypt(Token, AESKey, AppId);
        private string signature,timestamp,nonce;
        private static LogHelper logger = new LogHelper(typeof(WechatSecurityHelper));


        public WechatSecurityHelper(string signature, string timestamp, string nonce)
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
            string encryptMsg="";
            int result = wxcpt.EncryptMsg(msg, timestamp, nonce, ref encryptMsg);
            if (result != 0)
            {
                logger.Error("消息加密失败");
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
            int result = wxcpt.DecryptMsg(signature, timestamp, nonce, msg,ref decryptMsg);
            if (result != 0)
            {
                logger.Error("消息解密失败,result:"+result);
            }
            return decryptMsg;
        }
    }
}
