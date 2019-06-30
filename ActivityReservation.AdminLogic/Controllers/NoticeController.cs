using System;
using System.Linq.Expressions;
using ActivityReservation.AdminLogic.ViewModels;
using ActivityReservation.Business;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.AspNetMvc.MvcSimplePager;

namespace ActivityReservation.AdminLogic.Controllers
{
    /// <summary>
    /// 公告管理
    /// </summary>
    public class NoticeController : AdminBaseController
    {
        private readonly IBLLNotice _bLLNotice;

        public NoticeController(ILogger<NoticeController> logger, OperLogHelper operLogHelper, IBLLNotice bLLNotice) : base(logger, operLogHelper)
        {
            _bLLNotice = bLLNotice;
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
        public ActionResult List([FromQuery]SearchHelperModel search)
        {
            Expression<Func<Notice, bool>> whereExpression = (n => n.IsDeleted == false);
            if (!string.IsNullOrEmpty(search.SearchItem1))
            {
                whereExpression = n => n.IsDeleted == false && n.NoticeTitle.Contains(search.SearchItem1);
            }
            try
            {
                var list = _bLLNotice.Paged(search.PageIndex, search.PageSize,
                    whereExpression, n => n.NoticePublishTime, false);
                return View(list.ToPagedList());
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
        public ActionResult Create([FromForm]NoticeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
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
                    NoticePublisher = UserName,
                    NoticePublishTime = DateTime.UtcNow,
                    UpdateBy = UserName,
                    UpdateTime = DateTime.UtcNow
                };
                //
                if (!string.IsNullOrEmpty(n.NoticeCustomPath))
                {
                    if (n.NoticeCustomPath.EndsWith(".html"))
                    {
                        n.NoticeCustomPath = n.NoticeCustomPath.Substring(0, n.NoticeCustomPath.Length - 5); // trim end ".html"
                    }
                }
                else
                {
                    n.NoticeCustomPath = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                }
                var existStatus = _bLLNotice.Exist(nx => nx.NoticeCustomPath.ToLower().Equals(n.NoticeCustomPath.ToLower()));
                if (existStatus)
                {
                    n.NoticeCustomPath = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                }
                n.NoticePath = $"{n.NoticeCustomPath}.html";

                var c = _bLLNotice.Insert(n);
                if (c == 1)
                {
                    OperLogHelper.AddOperLog($"{UserName}添加新公告，{n.NoticeTitle},ID:{n.NoticeId:N}",
                        OperLogModule.Notice, UserName);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
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
                var existStatus = _bLLNotice.Exist(n => n.NoticePath.ToLower().Equals(path.ToLower()));
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
        public ActionResult Preview([FromForm]NoticeViewModel model)
        {
            return View(model);
        }

        public JsonResult Delete(Guid noticeId)
        {
            var result = _bLLNotice.Delete(new Notice() { NoticeId = noticeId });
            if (result > 0)
            {
                OperLogHelper.AddOperLog($"删除公告{noticeId:N}", OperLogModule.Notice, UserName);
                return Json("");
            }
            return Json("删除失败");
        }
    }
}
