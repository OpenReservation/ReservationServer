using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private static LogHelper logger = new LogHelper(typeof(ChatRobotHelper));

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
                if (String.IsNullOrEmpty(response))
                {
                    return "error";
                }
                else
                {
                    return response;
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                return "error";
            }
        }
    }
}
