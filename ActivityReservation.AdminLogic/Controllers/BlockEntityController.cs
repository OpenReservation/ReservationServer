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
    /// 黑名单
    /// </summary>
    public class BlockEntityController : AdminBaseController
    {
        // GET: Admin/BlockEntity
        public ActionResult Index()
        {
            var types = BusinessHelper.BlockTypeHelper.GetAll();
            return View(types);
        }

        /// <summary>
        /// 数据视图
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult List(SearchHelperModel search)
        {
            //默认查询全部
            Expression<Func<BlockEntity, bool>> whereLambda = (b => true);
            //判断查询条件
            if (!String.IsNullOrEmpty(search.SearchItem1) && !("0".Equals(search.SearchItem1)))
            {
                var id = Guid.Parse(search.SearchItem1);
                if (!String.IsNullOrEmpty(search.SearchItem2))
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
                if (!String.IsNullOrEmpty(search.SearchItem2))
                {
                    whereLambda = (b => b.BlockValue.Contains(search.SearchItem2));
                }
            }
            var rowsCount = 0;
            try
            {
                var blockList = BusinessHelper.BlockEntityHelper.GetPagedList(search.PageIndex, search.PageSize,
                    out rowsCount, whereLambda, b => b.BlockTime, false);
                var dataList = blockList.ToPagedList(search.PageIndex, search.PageSize, rowsCount);
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
        /// <param name="model">黑名单model</param>
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
                    BlockTime = DateTime.Now
                };
                try
                {
                    var count = BusinessHelper.BlockEntityHelper.Add(entity);
                    if (count == 1)
                    {
                        //记录日志
                        OperLogHelper.AddOperLog(String.Format("添加 {0} 到黑名单", blockValue), OperLogModule.BlockEntity,
                            Username);
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
        /// <param name="status">状态</param>
        /// <returns></returns>
        public ActionResult UpdateEntityStatus(Guid entityId, string entityName, int status)
        {
            var entity = new BlockEntity() { BlockId = entityId };
            if (status > 0)
            {
                entity.IsActive = true;
            }
            else
            {
                entity.IsActive = false;
            }
            try
            {
                var count = BusinessHelper.BlockEntityHelper.Update(entity, "IsActive");
                if (count > 0)
                {
                    OperLogHelper.AddOperLog(
                        String.Format("更改黑名单 {0} 状态为 {1}", entityName, entity.IsActive ? "启用" : "禁用"),
                        OperLogModule.BlockEntity, Username);
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
                var c = BusinessHelper.BlockEntityHelper.Delete(new BlockEntity() { BlockId = entityId });
                if (c == 1)
                {
                    //记录日志
                    OperLogHelper.AddOperLog($"删除黑名单 {entityName}", OperLogModule.BlockEntity, Username);
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return Json(false);
        }
    }
}