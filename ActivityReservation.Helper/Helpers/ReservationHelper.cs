using ActivityReservation.ViewModels;
using Business;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActivityReservation.Helpers
{
    public class ReservationHelper
    {
        /// <summary>
        /// 最多可预约天数
        /// </summary>
        private const int MaxReservationDiffDays = 7;

        /// <summary>
        /// 根据预约日期和预约地点获取可用的预约时间段
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <param name="placeId">预约地点id</param>
        /// <returns></returns>
        public static List<ReservationPeriodViewModel> GetAvailabelPeriodsByDateAndPlace(DateTime dt, Guid placeId)
        {
            //待审核和审核通过的预约时间段不能再被预约
            var reservationList = DependencyResolver.Current.GetService<IBLLReservation>().GetAll(r =>
                DbFunctions.DiffDays(r.ReservationForDate, dt) == 0 && r.ReservationPlaceId == placeId &&
                r.ReservationStatus != 2);
            var reservationPeriod = DependencyResolver.Current.GetService<IBLLReservationPeriod>().GetAll(_ => _.PlaceId == placeId);

            return reservationPeriod.Select(_ => new ReservationPeriodViewModel
            {
                PeriodId = _.PeriodId,
                PeriodIndex = _.PeriodIndex,
                PeriodTitle = _.PeriodTitle,
                PeriodDescription = _.PeriodDescription,
                IsCanReservate = reservationList.All(r => (r.ReservationPeriod & (1 << _.PeriodIndex)) == 0)
            }).OrderBy(_ => _.PeriodIndex).ThenBy(_ => _.PeriodTitle).ToList();
        }

        /// <summary>
        /// 判断预约日期是否在可预约范围内
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <param name="isAdmin">isAdmin</param>
        /// <param name="msg">errMsg</param>
        /// <returns></returns>
        public static bool IsReservationForDateAvailabel(DateTime dt, bool isAdmin, out string msg)
        {
            var daysdiff = dt.Subtract(DateTime.Today).Days;
            if (daysdiff < 0)
            {
                msg = "预约日期不可预约";
                return false;
            }
            if (!isAdmin && daysdiff > MaxReservationDiffDays)
            {
                msg = $"预约日期需要在{MaxReservationDiffDays}天内";
                return false;
            }

            var disabledPeriods = new BLLDisabledPeriod().GetAll(p =>
                !p.IsDeleted && p.IsActive && DbFunctions.DiffDays(p.StartDate, dt) >= 0 &&
                DbFunctions.DiffDays(dt, p.EndDate) >= 0);
            if (disabledPeriods == null || !disabledPeriods.Any())
            {
                msg = "";
                return true;
            }
            msg = "预约日期被禁用，如要预约请联系网站管理员";
            return false;
        }

        /// <summary>
        /// 判断预约时间段是否可用
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <param name="placeId">预约地点id</param>
        /// <param name="reservationForPeriodIds">预约时间段id</param>
        /// <returns></returns>
        private static bool IsReservationForPeriodAvailable(DateTime dt, Guid placeId, string reservationForPeriodIds)
        {
            var periods = GetAvailabelPeriodsByDateAndPlace(dt, placeId);
            // 预约时间段逻辑修改
            var periodIndexes = reservationForPeriodIds.Split(',').Select(_ => Convert.ToInt32(_)).ToArray();
            //
            if (periodIndexes.All(p => periods.Any(_ => _.IsCanReservate && _.PeriodIndex == p)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 提交信息是否在黑名单中
        /// </summary>
        /// <param name="reservation">预约信息</param>
        /// <param name="mesage">错误信息</param>
        /// <returns></returns>
        private static bool IsReservationInfoInBlockList(ReservationViewModel reservation, out string mesage)
        {
            var blockList = new BLLBlockEntity().GetAll(b => b.IsActive);
            mesage = "";
            //预约人手机号
            if (blockList.Any(b => b.BlockValue.Equals(reservation.ReservationPersonPhone)))
            {
                mesage = "手机号已被拉黑";
                return true;
            }
            //预约人IP地址
            var ip = HttpContext.Current.Request.UserHostAddress;
            if (blockList.Any(b => b.BlockValue.Equals(ip)))
            {
                mesage = "IP地址已被拉黑";
                return true;
            }
            //预约人姓名
            if (blockList.Any(b => b.BlockValue.Equals(reservation.ReservationPersonName)))
            {
                mesage = "预约人姓名已经被拉黑";
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断预约是否合法
        /// </summary>
        /// <param name="reservation">预约信息</param>
        /// <param name="msg">预约错误提示信息</param>
        /// <param name="isAdmin">是否是管理员预约</param>
        /// <returns></returns>
        public static bool IsReservationAvailabel(ReservationViewModel reservation, out string msg,
            bool isAdmin = false)
        {
            if (reservation == null || string.IsNullOrEmpty(reservation.ReservationPersonName) ||
                string.IsNullOrEmpty(reservation.ReservationPersonPhone) ||
                string.IsNullOrEmpty(reservation.ReservationForTimeIds) || Guid.Empty == reservation.ReservationPlaceId)
            {
                msg = "预约信息不完整";
                return false;
            }
            var reservationForDate = reservation.ReservationForDate;
            if (!IsReservationForDateAvailabel(reservationForDate, isAdmin, out msg))
            {
                return false;
            }
            if (!IsReservationForPeriodAvailable(reservationForDate, reservation.ReservationPlaceId,
                reservation.ReservationForTimeIds))
            {
                msg = "预约时间段冲突，请重新选择预约时间段";
                return false;
            }
            return !IsReservationInfoInBlockList(reservation, out msg);
        }
    }
}