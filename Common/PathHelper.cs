using System.Web.Hosting;

namespace Common
{
    public static class PathHelper
    {
        /// <summary>
        /// 将虚拟路径转换为物理路径
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <returns>虚拟路径对应的物理路径</returns>
        public static string MapPath(string virtualPath)
        {
            //拼接路径
            return HostingEnvironment.MapPath("~/") + virtualPath;
        }
    }
}