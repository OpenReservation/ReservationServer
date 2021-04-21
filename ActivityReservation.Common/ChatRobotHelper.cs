using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeihanLi.Extensions;

namespace ActivityReservation.Common
{
    /// <summary>
    /// ChatBotHelper
    /// </summary>
    public class ChatBotHelper
    {
        /// <summary>
        /// 青云客请求地址格式，详情参见 http://api.qingyunke.com/
        /// 0：message
        /// </summary>
        private const string QingyunkeRequestUrlFormat = "http://api.qingyunke.com/api.php?key=free&appid=0&msg={0}";

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger _logger;

        private readonly HttpClient _httpClient;

        public ChatBotHelper(HttpClient httpClient, ILogger<ChatBotHelper> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// 获取机器人回复【异步】
        /// </summary>
        /// <param name="request">请求</param>
        /// <param name="cancellationToken"></param>
        /// <returns>回复信息</returns>
        public async Task<string> GetBotReplyAsync(string request, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (request.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }
            try
            {
                var response = await _httpClient.
                    GetAsync(string.Format(QingyunkeRequestUrlFormat, request.UrlEncode()), cancellationToken);
                var responseText = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseText))
                {
                    var res = responseText.JsonToObject<QingyunkeResponseModel>();
                    if (res != null && res.Result == 0)
                    {
                        return res.Content;
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return "error";
        }

        private class QingyunkeResponseModel
        {
            /// <summary>
            /// result
            /// </summary>
            [JsonProperty("result")]
            public int Result { get; set; }

            private string _content;

            /// <summary>
            /// content
            /// </summary>
            [JsonProperty("content")]
            public string Content
            {
                get => _content;
                set
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        _content = value.Replace("{br}", "\n");
                    }
                }
            }
        }
    }
}
