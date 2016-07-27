using System.Collections.Generic;
using System.IO;

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
            return System.Web.Hosting.HostingEnvironment.MapPath("~/")+ virtualPath;
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="dir">目录名</param>
        /// <returns></returns>
        public static List<FileModel> GetFileList(string dir)
        {
            string[] files = System.IO.Directory.GetFiles(dir);
            List<FileModel>  fileList =new List<FileModel>();
            FileModel model = null;
            foreach (string item in files)
            {
                FileInfo info = new FileInfo(item);
                model = new FileModel() { CreateTime = info.CreationTime, FileName = info.Name, FileSize = info.Length / 1024 ,FileExtension=info.Extension,FilePath=info.FullName};
                fileList.Add(model);
            }
            return fileList;
        }
    }
}