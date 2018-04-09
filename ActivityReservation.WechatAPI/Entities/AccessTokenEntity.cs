using Newtonsoft.Json;

namespace ActivityReservation.WechatAPI.Entities
{
    internal class AccessTokenEntity
    {
        /// <summary>
        /// AccessToken
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// ExpiresIn(s)
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
