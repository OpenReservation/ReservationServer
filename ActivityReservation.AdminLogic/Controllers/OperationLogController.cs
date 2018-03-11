using ActivityReservation.Helpers;
using ActivityReservation.WorkContexts;
using Models;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using WeihanLi.AspNetMvc.MvcSimplePager;

namespace ActivityReservation.AdminLogic.Controllers
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class OperationLogController : AdminBaseController
    {
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
            Expression<Func<OperationLog, bool>> whereLambda = (l => 1 == 1);
            //日志模块名称
            if (!string.IsNullOrEmpty(search.SearchItem1))
            {
                //日志内容
                if (!string.IsNullOrEmpty(search.SearchItem2))
                {
                    whereLambda = (l =>
                        l.LogContent.Contains(search.SearchItem2) && l.LogModule.Contains(search.SearchItem1));
                }
                else
                {
                    whereLambda = (l => l.LogModule.Contains(search.SearchItem1));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(search.SearchItem2))
                {
                    whereLambda = (l => l.LogContent.Contains(search.SearchItem2));
                }
            }
            var rowsCount = 0;
            try
            {
                var logList = BusinessHelper.OperationLogHelper.GetPagedList(search.PageIndex, search.PageSize,
                    out rowsCount, whereLambda, l => l.OperTime, false);
                var dataList = logList.ToPagedList(search.PageIndex, search.PageSize, rowsCount);
                return View(dataList);
            }
            catch (Exception ex)
            {
                Logger.Error("ex");
                throw ex;
            }
        }
    }
}