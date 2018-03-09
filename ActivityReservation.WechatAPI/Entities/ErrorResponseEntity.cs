using Newtonsoft.Json;

namespace ActivityReservation.WechatAPI.Entities
{
    internal class ErrorResponseEntity
    {
        [JsonProperty("errcode")]
        public int ErrorCode { get; set; }

        [JsonProperty("errmsg")]
        public string ErrorMsg { get; set; }
    }
}
