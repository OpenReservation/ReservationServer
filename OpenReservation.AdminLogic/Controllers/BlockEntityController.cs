﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenReservation.Business;
using OpenReservation.Helpers;
using OpenReservation.Models;
using OpenReservation.WorkContexts;
using WeihanLi.Web.Pager;
using WeihanLi.Common.Helpers;
using WeihanLi.EntityFramework;
using WeihanLi.Redis;

namespace OpenReservation.AdminLogic.Controllers;

/// <summary>
/// 黑名单
/// </summary>
public class BlockEntityController : AdminBaseController
{
    // GET: Admin/BlockEntity
    public ActionResult Index()
    {
        return View(HttpContext.RequestServices.GetRequiredService<IBLLBlockType>().Select(_ => true));
    }

    public JsonResult BlockTypes()
    {
        return Json(HttpContext.RequestServices.GetRequiredService<IBLLBlockType>().Get());
    }

    /// <summary>
    /// 数据视图
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public ActionResult List(SearchHelperModel search)
    {
        //默认查询全部
        Expression<Func<BlockEntity, bool>> whereLambda = ExpressionHelper.True<BlockEntity>();
        //判断查询条件
        if (!string.IsNullOrEmpty(search.SearchItem1) && !("0".Equals(search.SearchItem1)))
        {
            var id = Guid.Parse(search.SearchItem1);
            if (!string.IsNullOrEmpty(search.SearchItem2))
            {
                whereLambda = (b => b.BlockTypeId == id && b.BlockValue.Contains(search.SearchItem2));
            }
            else
            {
                whereLambda = (b => b.BlockTypeId == id);
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(search.SearchItem2))
            {
                whereLambda = (b => b.BlockValue.Contains(search.SearchItem2));
            }
        }
        try
        {
            var blockList = _blockEntityHelper.GetPagedList(queryBuilder => queryBuilder
                .WithPredict(whereLambda)
                .WithInclude(q => q.Include(b => b.BlockType))
                .WithOrderBy(q => q.OrderByDescending(b => b.BlockTime)), search.PageIndex, search.PageSize);
            var dataList = blockList.ToPagedList();
            return View(dataList);
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            throw;
        }
    }

    /// <summary>
    /// 添加到黑名单
    /// </summary>
    /// <returns></returns>
    public ActionResult AddEntity(Guid typeId, string blockValue)
    {
        if (ModelState.IsValid)
        {
            var entity = new BlockEntity()
            {
                BlockId = Guid.NewGuid(),
                BlockTypeId = typeId,
                BlockValue = blockValue,
                BlockTime = DateTime.UtcNow
            };
            try
            {
                var count = _blockEntityHelper.Insert(entity);
                if (count == 1)
                {
                    ReloadBlackListCache();
                    //记录日志
                    OperLogHelper.AddOperLog($"添加 {blockValue} 到黑名单", OperLogModule.BlockEntity,
                        UserName);
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return Json(false);
        }
        else
        {
            return Json(false);
        }
    }

    /// <summary>
    /// 更新黑名单实体状态
    /// </summary>
    /// <param name="entityId">黑名单数据id</param>
    /// <param name="entityName"></param>
    /// <param name="status">状态</param>
    /// <returns></returns>
    public ActionResult UpdateEntityStatus(Guid entityId, string entityName, int status)
    {
        try
        {
            var count = _blockEntityHelper.Update(e => e.BlockId == entityId, e => e.IsActive, status > 0);
            if (count > 0)
            {
                ReloadBlackListCache();
                OperLogHelper.AddOperLog(
                    $"更改黑名单 {entityName} 状态为 {(status > 0 ? "启用" : "禁用")}",
                    OperLogModule.BlockEntity, UserName);
                return Json(true);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return Json(false);
    }

    /// <summary>
    /// 从黑名单中删除一条数据
    /// </summary>
    /// <param name="entityId">黑名单数据id</param>
    /// <param name="entityName">黑名单数据名称</param>
    /// <returns></returns>
    public ActionResult DeleteEntity(Guid entityId, string entityName)
    {
        try
        {
            var c = _blockEntityHelper.Delete(new BlockEntity() { BlockId = entityId });
            if (c == 1)
            {
                ReloadBlackListCache();
                //记录日志
                OperLogHelper.AddOperLog($"删除黑名单 {entityName}", OperLogModule.BlockEntity, UserName);
                return Json(true);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return Json(false);
    }

    public bool ReloadBlackListCache()
    {
        return _cacheClient.Set(Constants.BlackListCacheKey, _blockEntityHelper.Get(q => q.WithPredict(x => x.IsActive)), TimeSpan.FromDays(1));
    }

    private readonly IBLLBlockEntity _blockEntityHelper;
    private readonly ICacheClient _cacheClient;

    public BlockEntityController(ILogger<OperationLogController> logger, ICacheClient cacheClient, OperLogHelper operLogHelper, IBLLBlockEntity bLLBlockEntity) : base(logger, operLogHelper)
    {
        _cacheClient = cacheClient;
        _blockEntityHelper = bLLBlockEntity;
    }
}