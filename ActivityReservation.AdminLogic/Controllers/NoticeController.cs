using System;
using System.Linq.Expressions;
using ActivityReservation.AdminLogic.ViewModels;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.AspNetMvc.MvcSimplePager;
using WeihanLi.Common.Log;

namespace ActivityReservation.AdminLogic.Controllers
{
    /// <summary>
    /// 公告管理
    /// </summary>
    public class NoticeController : AdminBaseController
    {
        public NoticeController(ILogger<NoticeController> logger, OperLogHelper operLogHelper) : base(logger, operLogHelper)
        {
        }

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
            if (!string.IsNullOrEmpty(search.SearchItem1))
            {
                whereLamdba = n => n.IsDeleted == false && n.NoticeTitle.Contains(search.SearchItem1);
            }
            try
            {
                var list = BusinessHelper.NoticeHelper.GetPagedList(search.PageIndex, search.PageSize, out var count,
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
        // [ValidateInput(false)]
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
                if (!string.IsNullOrEmpty(n.NoticeCustomPath))
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
                    OperLogHelper.AddOperLog($"{Username}添加新公告，{n.NoticeTitle},ID:{n.NoticeId:N}",
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
            if (string.IsNullOrEmpty(path))
            {
                return Json(true);
            }
            try
            {
                if (!path.EndsWith(".html"))
                {
                    path = path + ".html";
                }
                var existStatus = BusinessHelper.NoticeHelper.Exist(n => n.NoticePath.ToLower().Equals(path.ToLower()));
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

        public JsonResult Delete(Guid noticeId)
        {
            var result = BusinessHelper.NoticeHelper.Delete(new Notice { NoticeId = noticeId });
            if (result > 0)
            {
                OperLogHelper.AddOperLog($"删除公告{noticeId:N}", OperLogModule.Notice, Username);
                return Json("");
            }
            return Json("删除失败");
        }
    }
}
