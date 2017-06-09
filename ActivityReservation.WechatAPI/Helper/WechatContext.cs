namespace ActivityReservation.WechatAPI.Helper
{
    internal class WechatContext
    {
        private readonly WechatSecurityHelper securityHelper;
        private readonly string requestMessage;

        public WechatContext(Model.WechatMsgRequestModel request , string msg)
        {
            securityHelper = new WechatSecurityHelper(request.Signature , request.Timestamp , request.Nonce);
            requestMessage = msg;
        }

        /// <summary>
        /// 发送消息用户id
        /// </summary>
        public string FromUserId { get; private set; }

        public string GetResponse()
        {
            string msg = securityHelper.DecryptMsg(requestMessage);
            string response = WechatMsgHelper.ReturnMessage(msg);
            return securityHelper.EncryptMsg(response);
        }
    }
}