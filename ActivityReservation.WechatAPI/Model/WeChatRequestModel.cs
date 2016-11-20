using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityReservation.WechatAPI.Model
{
    public class WeChatRequestValidModel
    {
        public string signature { get; set; }
        public string timestamp { get; set; }
        public string nonce { get; set; }

        public string echostr { get; set; }
    }

    /// <summary>
    /// 微信推送消息模型
    /// </summary>
    public class WeChatMsgRequestModel
    {
        public string timestamp { get; set; }
        public string nonce { get; set; }

        public string msg_signature { get; set; }
    }
}
