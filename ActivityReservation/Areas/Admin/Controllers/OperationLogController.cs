using ActivityReservation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ActivityReservation.Areas.Admin.Controllers
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [Authorize]
    [Filters.AdminPermissionRequired]
    public class OperationLogController : Controller
    {
        /// <summary>
        /// logger
        /// </summary>
        private static Common.LogHelper logger = new Common.LogHelper(typeof(OperationLogController));

        /// <summary>
        /// 操作日志首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 操作日志数据列表
        /// </summary>
        /// <param name="search">查询搜索条件</param>
        /// <returns></returns>
        public ActionResult List(SearchHelperModel search)
        {
            Expression<Func<Models.OperationLog, bool>> whereLambda = (l => 1 == 1);
            //日志模块名称
            if (!String.IsNullOrEmpty(search.SearchItem1))
            {
                //日志内容
                if (!String.IsNullOrEmpty(search.SearchItem2))
                {
                    whereLambda = (l => l.LogContent.Contains(search.SearchItem2) && l.LogModule.Contains(search.SearchItem1));
                }
                else
                {
                    whereLambda = (l => l.LogModule.Contains(search.SearchItem1));
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(search.SearchItem2))
                {
                    whereLambda = (l => l.LogContent.Contains(search.SearchItem2));
                }
            }
            int rowsCount = 0;
            try
            {
                List<Models.OperationLog> logList = new Business.BLLOperationLog().GetPagedList(search.PageIndex, search.PageSize, out rowsCount, whereLambda, l => l.OperTime, false);
                PagerModel pager = new PagerModel(search.PageIndex, search.PageSize, rowsCount);
                PagedListModel<Models.OperationLog> dataList = new PagedListModel<Models.OperationLog>() { Data = logList, Pager = pager };
                return View(dataList);
            }
            catch (Exception ex)
            {
                logger.Error("ex");
                throw ex;
            }
        }
    }
}