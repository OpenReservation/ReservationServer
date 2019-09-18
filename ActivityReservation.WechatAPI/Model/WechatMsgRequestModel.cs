using System.ComponentModel.DataAnnotations;

namespace ActivityReservation.WechatAPI.Model
{
    /// <summary>
    /// 微信推送消息模型
    /// </summary>
    public class WechatMsgRequestModel
    {
        [Required]
        public string Timestamp { get; set; }

        [Required]
        public string Nonce { get; set; }

        [Required]
        public string Signature { get; set; }

        public string Msg_Signature { get; set; }

        public string RequestContent { get; set; }
    }
}
