using System.Web;

namespace Common
{
    /// <summary>
    /// 请求帮助类 
    /// </summary>
    public class RequestHelper
    {
        /// <summary>
        /// 获取请求IP地址 
        /// </summary>
        /// <returns></returns>
        public static string GetRequestIP()
        {
            return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                   HttpContext.Current.Request.UserHostAddress;
        }
    }
}