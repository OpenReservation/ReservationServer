using Exceptionless.Json;
using System;
using System.Threading.Tasks;
using WeihanLi.Common.Helpers;

namespace Common
{
    /// <summary>
    /// ChatBotHelper
    /// </summary>
    public static class ChatRobotHelper
    {
        /// <summary>
        /// 青云客请求地址格式，详情参见 http://api.qingyunke.com/
        /// 0：message
        /// </summary>
        private const string QingyunkeRequestUrlFormat = "http://api.qingyunke.com/api.php?key=free&appid=0&msg={0}";

        /// <summary>
        /// logger
        /// </summary>
        private static readonly LogHelper Logger = new LogHelper(typeof(ChatRobotHelper));

        /// <summary>
        /// 获取机器人回复
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>回复信息</returns>
        public static string GetBotReply(string request)
        {
            try
            {
                var response = HttpHelper.HttpGetString(String.Format(QingyunkeRequestUrlFormat, request));
                if (!string.IsNullOrEmpty(response))
                {
                    var res = ConvertHelper.JsonToObject<QingyunkeResponseModel>(response);
                    if (res != null && res.Result == 0)
                    {
                        return res.Content;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return "error";
        }

        /// <summary>
        /// 获取机器人回复【异步】
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns>回复信息</returns>
        public static async Task<string> GetBotReplyAsync(string request)
        {
            try
            {
                var response = await HttpHelper.HttpGetStringAsync(String.Format(QingyunkeRequestUrlFormat, request));
                if (!string.IsNullOrEmpty(response))
                {
                    var res = ConvertHelper.JsonToObject<QingyunkeResponseModel>(response);
                    if (res != null && res.Result == 0)
                    {
                        return res.Content;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return "error";
        }
    }

    internal class QingyunkeResponseModel
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