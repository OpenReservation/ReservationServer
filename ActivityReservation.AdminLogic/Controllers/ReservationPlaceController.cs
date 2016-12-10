using Models;
using MvcSimplePager;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using ActivityReservation.Helpers;

namespace ActivityReservation.AdminLogic.Controllers
{
    /// <summary>
    /// 预约活动室管理 
    /// </summary>

    public class ReservationPlaceController : BaseAdminController
    {
        /// <summary>
        /// 活动室管理首页 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() => View();

        /// <summary>
        /// 活动室列表页面 
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string placeName, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0)
            {
                pageIndex = 1;
            }
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            Expression<Func<Models.ReservationPlace, bool>> whereLambda = (p => p.IsDel == false);
            if (!String.IsNullOrEmpty(placeName))
            {
                whereLambda = (p => p.PlaceName.Contains(placeName) && p.IsDel == false);
            }
            int totalCount = 0;
            var list = BusinessHelper.ReservationPlaceHelper.GetPagedList(pageIndex, pageSize, out totalCount, whereLambda, p => p.UpdateTime, false);
            var data = list.ToPagedList(pageIndex, pageSize, totalCount);
            return View(data);
        }

        /// <summary>
        /// 更新活动室名称
        /// </summary>
        /// <param name="placeId">活动室id</param>
        /// <param name="newName">修改后的活动室名称</param>
        /// <param name="beforeName">修改之前的活动室名称</param>
        /// <returns></returns>
        public ActionResult UpdatePlaceName(Guid placeId,string newName,string beforeName)
        {
            if (String.IsNullOrEmpty(newName))
            {
                return Json("活动室名称不能为空");
            }
            if (!BusinessHelper.ReservationPlaceHelper.Exist(p => p.PlaceId == placeId))
            {
                return Json("活动室不存在");
            }
            if (BusinessHelper.ReservationPlaceHelper.Exist(p => p.PlaceName.ToUpperInvariant().Equals(newName.ToUpperInvariant()) && p.IsDel == false))
            {
                return Json("活动室名称已存在");
            }
            try
            {
                BusinessHelper.ReservationPlaceHelper.Update(new ReservationPlace() {PlaceId = placeId,PlaceName = newName,UpdateBy = Username,UpdateTime = DateTime.Now},"PlaceName", "UpdateBy", "UpdateTime");
                OperLogHelper.AddOperLog($"更新活动室 {placeId.ToString()} 名称，从 {beforeName} 修改为{newName}", Module.ReservationPlace, Username);
                return Json("");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json("更新活动室名称失败，发生异常：" + ex.Message);
            }
        }

        /// <summary>
        /// 添加活动室 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddPlace(string placeName)
        {
            if (!String.IsNullOrEmpty(placeName))
            {
                if (BusinessHelper.ReservationPlaceHelper.Exist(p => p.PlaceName.ToUpperInvariant().Equals(placeName.ToUpperInvariant()) && p.IsDel == false))
                {
                    return Json("活动室已存在");
                }
                ReservationPlace place = new ReservationPlace()
                {
                    PlaceId = Guid.NewGuid(),
                    PlaceName = placeName,
                    UpdateBy = Username
                };
                try
                {
                    BusinessHelper.ReservationPlaceHelper.Add(place);
                    //记录日志
                    OperLogHelper.AddOperLog(String.Format("新增活动室：{0}",placeName), Module.ReservationPlace, place.UpdateBy);
                    return Json("");
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    return Json("添加失败，出现异常："+ex.Message);
                }
            }
            else
            {
                return Json("活动室名称不能为空");
            }
        }

        /// <summary>
        /// 删除活动室 
        /// </summary>
        /// <param name="placeId"> 活动室id </param>
        /// <param name="placeName"> 活动室名称 </param>
        /// <returns></returns>
        public JsonResult DeletePlace(Guid placeId, string placeName)
        {
            if (String.IsNullOrEmpty(placeName))
            {
                return Json("活动室名称不能为空");
            }
            if (!BusinessHelper.ReservationPlaceHelper.Exist(p => p.PlaceId == placeId))
            {
                return Json("活动室不存在");
            }
            try
            {
                BusinessHelper.ReservationPlaceHelper.Update(new ReservationPlace() { PlaceId = placeId,IsDel = true,UpdateBy = Username},"IsDel", "UpdateBy", "UpdateTime");
                OperLogHelper.AddOperLog($"删除活动室{placeId.ToString()}:{placeName}", Module.ReservationPlace, Username);
                return Json("");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json("删除活动室失败，发生异常："+ex.Message);
            };
        }

        /// <summary>
        /// 删除活动室 
        /// </summary>
        /// <param name="placeId"> 活动室id </param>
        /// <param name="placeName"> 活动室名称 </param>
        /// <param name="status">活动室状态，大于0启用，否则禁用</param>
        /// <returns></returns>
        public JsonResult UpdatePlaceStatus(Guid placeId, string placeName,int status)
        {
            if (String.IsNullOrEmpty(placeName))
            {
                return Json("活动室名称不能为空");
            }
            if (!BusinessHelper.ReservationPlaceHelper.Exist(p => p.PlaceId == placeId))
            {
                return Json("活动室不存在");
            }
            try
            {
                bool bStatus = (status > 0);
                BusinessHelper.ReservationPlaceHelper.Update(new ReservationPlace() { PlaceId = placeId,UpdateBy = Username, IsActive = bStatus }, "IsActive","UpdateBy","UpdateTime");
                OperLogHelper.AddOperLog(String.Format("修改活动室{0}:{1}状态，{2}", placeId.ToString(),placeName,(status > 0)?"启用":"禁用"), Module.ReservationPlace, Username);
                return Json("");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json("修改活动室状态失败，发生异常：" + ex.Message);
            };
        }
    }
}