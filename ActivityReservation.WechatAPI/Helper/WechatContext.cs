namespace ActivityReservation.WechatAPI.Helper
{
    internal class WechatContext
    {
        private static Common.LogHelper logger = new Common.LogHelper(typeof(WechatContext));
        private readonly WechatSecurityHelper securityHelper;
        private readonly string requestMessage;

        public WechatContext(Model.WechatMsgRequestModel request , string msg)
        {
            logger.Debug("微信服务器消息：" + Newtonsoft.Json.JsonConvert.SerializeObject(request)+","+msg);
            securityHelper = new WechatSecurityHelper(request.Signature , request.Timestamp , request.Nonce);
            requestMessage = securityHelper.DecryptMsg(msg);
            logger.Debug("收到微信消息："+requestMessage);
        }

        /// <summary>
        /// 发送消息用户id
        /// </summary>
        public string FromUserId { get; private set; }

        public string GetResponse()
        {
            string response = WechatMsgHelper.ReturnMessage(requestMessage);
            logger.Debug("返回消息：" + response);
            return securityHelper.EncryptMsg(response);
        }
    }
}