using ActivityReservation.Helpers;
using WeihanLi.AspNetMvc.MvcSimplePager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ActivityReservation.WorkContexts;

namespace ActivityReservation.AdminLogic.Controllers
{
    /// <summary>
    /// 公告管理
    /// </summary>
    public class NoticeController: AdminBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 公告列表
        /// </summary>
        /// <param name="search">查询信息</param>
        /// <returns></returns>
        public ActionResult List(SearchHelperModel search)
        {
            Expression<Func<Models.Notice, bool>> whereLamdba = (n => n.IsDeleted == false);
            if (!String.IsNullOrEmpty(search.SearchItem1))
            {
                whereLamdba = (n => n.IsDeleted == false && n.NoticeTitle.Contains(search.SearchItem1));
            }
            int count = -1;
            try
            {
                List<Models.Notice> list = BusinessHelper.NoticeHelper.GetPagedList(search.PageIndex, search.PageSize, out count, whereLamdba, n => n.NoticePublishTime, false);
                return View(list.ToPagedList(search.PageIndex , search.PageSize , count));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
        
        /// <summary>
        /// 添加公告
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 保存公告
        /// </summary>
        /// <param name="model">公告信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ViewModels.NoticeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                //notice
                Models.Notice n = new Models.Notice()
                {
                    NoticeId = Guid.NewGuid(),
                    CheckStatus = true,//默认审核通过
                    NoticeContent = model.Content,
                    NoticeTitle = model.Title,
                    NoticeCustomPath = model.CustomPath,
                    NoticePublisher = Username,
                    NoticePublishTime = DateTime.Now,
                    UpdateBy = Username,
                    UpdateTime = DateTime.Now
                };
                //
                if (!String.IsNullOrEmpty(n.NoticeCustomPath))
                {
                    if (n.NoticeCustomPath.EndsWith(".html"))
                    {
                        n.NoticePath = n.NoticeCustomPath;
                    }
                    else
                    {
                        n.NoticePath = n.NoticeCustomPath+".html";
                    }
                }
                else
                {
                    n.NoticePath = DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";
                }
                int c = BusinessHelper.NoticeHelper.Add(n);
                if (c == 1)
                {
                    OperLogHelper.AddOperLog(String.Format("{0}添加新公告，{1}", Username, n.NoticeTitle), OperLogModule.Notice, Username);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// 判断路径是否可用
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public JsonResult IsPathAvailable(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return Json(true);
            }
            try
            {
                if (!path.EndsWith(".html"))
                {
                    path = path + ".html";
                }
                bool existStatus = new Business.BLLNotice().Exist(n => n.NoticePath.ToLower().Equals(path.ToLower()));
                if (existStatus)
                {
                    return Json(false);
                }
                else
                {
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult Preview(ViewModels.NoticeViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            return View();
        }
    }
}