using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenReservation.Business;
using OpenReservation.Helpers;
using OpenReservation.Models;
using OpenReservation.WorkContexts;
using WeihanLi.Web.AccessControlHelper;
using WeihanLi.Web.Pager;
using WeihanLi.Extensions;

namespace OpenReservation.AdminLogic.Controllers;

/// <summary>
/// 操作日志
/// </summary>
[Authorize(AccessControlHelperConstants.PolicyName)]
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
        Expression<Func<OperationLog, bool>> whereLambda = (l => true);

        if (!string.IsNullOrWhiteSpace(search.SearchItem1)) // 日志模块名称
        {
            whereLambda = whereLambda.And((l => l.LogModule == search.SearchItem1.Trim()));
        }
        if (!string.IsNullOrWhiteSpace(search.SearchItem2)) // 日志内容
        {
            whereLambda = whereLambda.And(l => l.LogContent.Contains(search.SearchItem2.Trim()));
        }
        var logList = operationLogHelper.Paged(search.PageIndex, search.PageSize,
            whereLambda, l => l.OperTime);
        var dataList = logList.ToPagedList();
        return View(dataList);
    }

    private readonly IBLLOperationLog operationLogHelper;

    public OperationLogController(ILogger<OperationLogController> logger, OperLogHelper operLogHelper, IBLLOperationLog bLLOperationLog) : base(logger, operLogHelper)
    {
        operationLogHelper = bLLOperationLog;
    }
}