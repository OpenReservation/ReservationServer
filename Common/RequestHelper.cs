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
            => WeihanLi.Common.Helpers.RequestHelper.GetIP();
    }
}