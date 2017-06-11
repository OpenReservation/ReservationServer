using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;

namespace ActivityReservation.WechatAPI.Helper
{
    public class WechatContext: MessageContext<IRequestMessageBase, IResponseMessageBase>
    {
        private static Common.LogHelper logger = new Common.LogHelper(typeof(WechatContext));
        private WechatSecurityHelper securityHelper;
        private string requestMessage,responseMessage;

        public WechatContext()
        {
        }

        public WechatContext(Model.WechatMsgRequestModel request)
        {
            logger.Debug("微信服务器消息：" + Newtonsoft.Json.JsonConvert.SerializeObject(request));
            securityHelper = new WechatSecurityHelper(request.Msg_Signature, request.Timestamp, request.Nonce);
            requestMessage = securityHelper.DecryptMsg(request.RequestContent);
            logger.Debug("收到微信消息：" + requestMessage);
            responseMessage = WechatMsgHandler.ReturnMessage(requestMessage);
            logger.Debug("返回消息：" + responseMessage);
        }

        public string GetResponse()
        {
            return securityHelper.EncryptMsg(responseMessage);
        }
    }
}