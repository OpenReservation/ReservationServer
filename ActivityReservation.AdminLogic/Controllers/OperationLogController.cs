using System;
using System.Linq.Expressions;
using ActivityReservation.Business;
using ActivityReservation.Helpers;
using ActivityReservation.Models;
using ActivityReservation.WorkContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeihanLi.AspNetMvc.MvcSimplePager;
using WeihanLi.Common.Helpers;

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
            try
            {
                var logList = operationLogHelper.Paged(search.PageIndex, search.PageSize,
                     whereLambda, l => l.OperTime, false);
                var dataList = logList.ToPagedList(search.PageIndex, search.PageSize, logList.TotalCount);
                return View(dataList);
            }
            catch (Exception ex)
            {
                Logger.Error("ex");
                throw ex;
            }
        }

        private readonly IBLLOperationLog operationLogHelper;

        public OperationLogController(ILogger<OperationLogController> logger, OperLogHelper operLogHelper, IBLLOperationLog bLLOperationLog) : base(logger, operLogHelper)
        {
            operationLogHelper = bLLOperationLog;
        }
    }
}
