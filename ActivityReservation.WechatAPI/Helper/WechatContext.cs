using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using WeihanLi.Common.Helpers;

namespace ActivityReservation.WechatAPI.Helper
{
    public class WechatContext: MessageContext<IRequestMessageBase, IResponseMessageBase>
    {
        private static LogHelper logger = new LogHelper(typeof(WechatContext));
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
        }

        public string GetResponseAsync()
        {
            responseMessage = WechatMsgHandler.ReturnMessage(requestMessage);
            logger.Debug("返回消息：" + responseMessage);
            return securityHelper.EncryptMsg(responseMessage);
        }
    }
}