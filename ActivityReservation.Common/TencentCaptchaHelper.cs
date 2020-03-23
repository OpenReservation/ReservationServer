using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeihanLi.Extensions;

namespace ActivityReservation.Common
{
    public class TencentCaptchaOptions
    {
        /// <summary>
        /// 客户端AppId
        /// </summary>
        [Required]
        public string AppId { get; set; }

        /// <summary>
        /// App Secret Key
        /// </summary>
        [Required]
        public string AppSecret { get; set; }
    }

    public class TencentCaptchaRequest
    {
        /// <summary>
        /// 验证码客户端验证回调的票据
        /// </summary>
        public string Ticket { get; set; }

        /// <summary>
        /// 验证码客户端验证回调的随机串
        /// </summary>
        public string Nonce { get; set; }

        /// <summary>
        /// 提交验证的用户的IP地址（eg: 10.127.10.2）
        /// </summary>
        public string UserIP { get; set; }
    }

    public class TencentCaptchaHelper
    {
        private class TencentCaptchaResponse
        {
            /// <summary>
            /// 1:验证成功，0:验证失败，100:AppSecretKey参数校验错误
            /// </summary>
            [JsonProperty("response")]
            public int Code { get; set; }

            /// <summary>
            /// 恶意等级 [0, 100]
            /// </summary>
            [JsonProperty("evil_level")]
            public string EvilLevel { get; set; }

            /// <summary>
            /// 错误信息
            /// </summary>
            [JsonProperty("err_msg")]
            public string ErrorMsg { get; set; }
        }

        private const string TencentCaptchaVerifyUrl = "https://ssl.captcha.qq.com/ticket/verify";
        private readonly TencentCaptchaOptions _captchaOptions;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public TencentCaptchaHelper(
            IOptions<TencentCaptchaOptions> option,
            ILogger<TencentCaptchaHelper> logger,
            HttpClient httpClient)
        {
            _captchaOptions = option.Value;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<bool> IsValidRequestAsync(TencentCaptchaRequest request)
        {
            // 参考文档：https://007.qq.com/captcha/#/gettingStart
            var response = await _httpClient.GetAsync(
                $"{TencentCaptchaVerifyUrl}?aid={_captchaOptions.AppId}&AppSecretKey={_captchaOptions.AppSecret}&Ticket={request.Ticket}&Randstr={request.Nonce}&UserIP={request.UserIP}");
            var responseText = await response.Content.ReadAsStringAsync();
            if (responseText.IsNotNullOrEmpty())
            {
                _logger.Debug($"Tencent captcha verify response:{responseText}");
                var result = responseText.JsonToObject<TencentCaptchaResponse>();
                if (result.Code == 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
