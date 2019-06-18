using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using ActivityReservation.Helpers;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Helpers;
using WeihanLi.Extensions;

namespace ActivityReservation.AdminLogic.Controllers
{
    public class HomeController : AdminBaseController
    {
        private string SiteUrl => Request.Scheme + "://" + Request.Host.Value;

        public ActionResult Index()
        {
            return View();
        }

        public void UploadFile()
        {
            //file root dir path 文件保存目录路径
            var savePath = "/Upload/";
            //file root dir url 文件保存目录URL
            var saveUrl = SiteUrl + savePath;
            //定义允许上传的文件扩展名
            var extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");
            //最大文件大小
            var maxSize = 1000000;
            var imgFile = Request.Form.Files["imgFile"];
            if (imgFile?.FileName == null)
            {
                showError("请选择文件。");
                return;
            }
            var dirPath = ApplicationHelper.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                showError("上传目录不存在。");
                return;
            }
            var dirName = Request.Query["dir"][0];
            if (string.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                showError("目录名不正确。");
                return;
            }
            var fileExt = Path.GetExtension(imgFile.FileName).ToLower();
            if (imgFile.Length > maxSize)
            {
                showError("上传文件大小超过限制。");
                return;
            }
            if (string.IsNullOrEmpty(fileExt) ||
                Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                showError($"上传文件扩展名是不允许的扩展名。\n只允许{extTable[dirName]}格式。");
            }
            //创建文件夹
            dirPath += dirName + "/";
            saveUrl += dirName + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            var ymd = DateTime.UtcNow.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            dirPath += ymd + "/";
            saveUrl += ymd + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            var newFileName = DateTime.UtcNow.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            var filePath = dirPath + newFileName;
            //save file
            using (var stream = System.IO.File.Create(filePath))
            {
                imgFile.CopyTo(stream);
            }
            //file access url 文件url
            var fileUrl = saveUrl + newFileName;
            var hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;
            Response.Body.Write(hash.ToJson().GetBytes());
        }

        [NonAction]
        private void showError(string message)
        {
            var hash = new Hashtable
            {
                ["error"] = 1,
                ["message"] = message
            };
            HttpContext.Response.Body.Write(hash.ToJson().GetBytes());
        }

        public ActionResult FileManager()
        {
            //根目录路径，相对路径
            var rootPath = "/Upload/";
            //根目录URL，可以指定绝对路径，比如 http://www.yoursite.com/upload/
            var rootUrl = SiteUrl + rootPath;
            //图片扩展名
            var fileTypes = "gif,jpg,jpeg,png,bmp";
            var currentPath = "";
            var currentUrl = "";
            var currentDirPath = "";
            var moveupDirPath = "";

            var dirPath = ApplicationHelper.MapPath(rootPath);
            var dirName = Request.Query["dir"][0];
            if (!string.IsNullOrEmpty(dirName))
            {
                if (Array.IndexOf("image,flash,media,file".Split(','), dirName) == -1)
                {
                    return Content("Invalid Directory name.");
                }
                dirPath += dirName + "/";
                rootUrl += dirName + "/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }
            //根据path参数，设置各路径和URL
            var path = Request.Query["path"][0];
            path = string.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = dirPath;
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = dirPath + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            var order = Request.Query["order"][0];
            order = string.IsNullOrEmpty(order) ? "" : order.ToLower();
            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                return Content("Access is not allowed.");
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                return Content("Parameter is not valid.");
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                return Content("Directory does not exist.");
            }
            //遍历目录取得文件信息
            var dirList = Directory.GetDirectories(currentPath);
            var fileList = Directory.GetFiles(currentPath);
            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;

                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;

                case "name":
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }
            var result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            var dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            for (var i = 0; i < dirList.Length; i++)
            {
                var dir = new DirectoryInfo(dirList[i]);
                var hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (var i = 0; i < fileList.Length; i++)
            {
                var file = new FileInfo(fileList[i]);
                var hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            return Json(result);
        }

        public HomeController(ILogger<HomeController> logger, OperLogHelper operLogHelper) : base(logger, operLogHelper)
        {
        }

        private class NameSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                var xInfo = new FileInfo(x.ToString());
                var yInfo = new FileInfo(y.ToString());
                return xInfo.FullName.CompareTo(yInfo.FullName);
            }
        }

        private class SizeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                var xInfo = new FileInfo(x.ToString());
                var yInfo = new FileInfo(y.ToString());
                return xInfo.Length.CompareTo(yInfo.Length);
            }
        }

        private class TypeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                var xInfo = new FileInfo(x.ToString());
                var yInfo = new FileInfo(y.ToString());
                return xInfo.Extension.CompareTo(yInfo.Extension);
            }
        }
    }
}
