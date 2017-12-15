using ActivityReservation.AdminLogic.ViewModels;
using ActivityReservation.Helpers;
using ActivityReservation.WorkContexts;
using Business;
using Models;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using WeihanLi.AspNetMvc.MvcSimplePager;

namespace ActivityReservation.AdminLogic.Controllers
{
    /// <summary>
    /// 公告管理
    /// </summary>
    public class NoticeController : AdminBaseController
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
            Expression<Func<Notice, bool>> whereLamdba = (n => n.IsDeleted == false);
            if (!String.IsNullOrEmpty(search.SearchItem1))
            {
                whereLamdba = (n => n.IsDeleted == false && n.NoticeTitle.Contains(search.SearchItem1));
            }
            var count = -1;
            try
            {
                var list = BusinessHelper.NoticeHelper.GetPagedList(search.PageIndex, search.PageSize, out count,
                    whereLamdba, n => n.NoticePublishTime, false);
                return View(list.ToPagedList(search.PageIndex, search.PageSize, count));
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
        public ActionResult Create(NoticeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                //notice
                var n = new Notice()
                {
                    NoticeId = Guid.NewGuid(),
                    CheckStatus = true, //默认审核通过
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
                        n.NoticePath = n.NoticeCustomPath + ".html";
                    }
                }
                else
                {
                    n.NoticePath = DateTime.Now.ToString("yyyyMMddHHmmss") + ".html";
                }
                var c = BusinessHelper.NoticeHelper.Add(n);
                if (c == 1)
                {
                    OperLogHelper.AddOperLog(String.Format("{0}添加新公告，{1}", Username, n.NoticeTitle),
                        OperLogModule.Notice, Username);
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
                var existStatus = new BLLNotice().Exist(n => n.NoticePath.ToLower().Equals(path.ToLower()));
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
        public ActionResult Preview(NoticeViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            return View();
        }
    }
}