using System;
using System.Collections.Generic;
using System.Linq;
using ActivityReservation.Business;
using ActivityReservation.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WeihanLi.Common;
using WeihanLi.Extensions;
using WeihanLi.Web.Extensions;

namespace ActivityReservation.Helpers
{
    public class ReservationHelper
    {
        /// <summary>
        /// 最多可预约天数
        /// </summary>
        private const int MaxReservationDiffDays = 7;

        private readonly IBLLReservationPeriod _bllReservationPeriod;
        private readonly IBLLReservation _bllReservation;
        private readonly IBLLDisabledPeriod _bllDisabledPeriod;
        private readonly IBLLBlockEntity _bllBlockEntity;

        public ReservationHelper(IBLLReservationPeriod bllReservationPeriod, IBLLReservation bllReservation, IBLLDisabledPeriod bllDisabledPeriod, IBLLBlockEntity bllBlockEntity)
        {
            _bllReservationPeriod = bllReservationPeriod;
            _bllReservation = bllReservation;
            _bllBlockEntity = bllBlockEntity;
            _bllDisabledPeriod = bllDisabledPeriod;
        }

        /// <summary>
        /// 根据预约日期和预约地点获取可用的预约时间段
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <param name="placeId">预约地点id</param>
        /// <returns></returns>
        public List<ReservationPeriodViewModel> GetAvailablePeriodsByDateAndPlace(DateTime dt, Guid placeId)
        {
            //待审核和审核通过的预约时间段不能再被预约
            var reservationList = _bllReservation.Select(r => r.ReservationPlaceId == placeId
                && r.ReservationStatus != 2);

            var reservationPeriod = _bllReservationPeriod
                .Select(_ => _.PlaceId == placeId)
                .OrderBy(_ => _.PeriodIndex)
                .ThenBy(_ => _.CreateTime);

            return reservationPeriod.Select((_, index) => new ReservationPeriodViewModel
            {
                PeriodIdx = index,
                PeriodId = _.PeriodId,
                PeriodIndex = _.PeriodIndex,
                PeriodTitle = _.PeriodTitle,
                PeriodDescription = _.PeriodDescription,
                IsCanReservate = reservationList.All(r => (r.ReservationPeriod & (1 << _.PeriodIndex)) == 0)
            }).OrderBy(_ => _.PeriodIndex).ToList();
        }

        /// <summary>
        /// 判断预约日期是否在可预约范围内以及所要预约的日期是否被禁用
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <param name="isAdmin">isAdmin</param>
        /// <param name="msg">errMsg</param>
        /// <returns></returns>
        public bool IsReservationForDateAvailable(DateTime dt, bool isAdmin, out string msg)
        {
            var daysDiff = dt.Subtract(DateTime.UtcNow.AddHours(8).Date).Days;// 时间转换
            if (daysDiff < 0)
            {
                msg = "预约日期不可预约";
                return false;
            }
            if (!isAdmin && daysDiff > MaxReservationDiffDays)
            {
                msg = $"预约日期需要在{MaxReservationDiffDays}天内";
                return false;
            }

            var disabledPeriods = _bllDisabledPeriod.Select(p =>
                !p.IsDeleted && p.IsActive).Where(p => dt > p.StartDate && dt <= p.EndDate).ToArray();
            if (disabledPeriods.Length == 0)
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
        private bool IsReservationForPeriodAvailable(DateTime dt, Guid placeId, string reservationForPeriodIds)
        {
            var periods = GetAvailablePeriodsByDateAndPlace(dt, placeId);
            // 预约时间段逻辑修改
            var periodIndexes = reservationForPeriodIds.SplitArray<int>();
            if (periodIndexes.All(p => periods.Any(_ => _.IsCanReservate && _.PeriodIndex == p)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 预约信息是否在黑名单中
        /// </summary>
        /// <param name="reservation">预约信息</param>
        /// <param name="message">错误信息</param>
        /// <returns></returns>
        private bool IsReservationInfoInBlockList(ReservationViewModel reservation, out string message)
        {
            var blockList = _bllBlockEntity.Select(b => b.IsActive);
            message = "";
            //预约人手机号
            if (blockList.Any(b => b.BlockValue.Equals(reservation.ReservationPersonPhone)))
            {
                message = "手机号已被拉黑";
                return true;
            }
            //预约人IP地址
            var ip = DependencyResolver.Current.GetService<IHttpContextAccessor>().HttpContext.GetUserIP();
            if (blockList.Any(b => b.BlockValue.Equals(ip)))
            {
                message = "IP地址已被拉黑";
                return true;
            }
            //预约人姓名
            if (blockList.Any(b => b.BlockValue.Equals(reservation.ReservationPersonName)))
            {
                message = "预约人姓名已经被拉黑";
                return true;
            }
            return false;
        }

        private readonly object _lock = new object();

        /// <summary>
        /// 判断预约是否合法
        /// </summary>
        /// <param name="reservation">预约信息</param>
        /// <param name="msg">预约错误提示信息</param>
        /// <param name="isAdmin">是否是管理员预约</param>
        /// <returns></returns>
        public bool IsReservationAvailable(ReservationViewModel reservation, out string msg,
            bool isAdmin = false)
        {
            if (reservation == null ||
                string.IsNullOrEmpty(reservation.ReservationPersonName) ||
                string.IsNullOrEmpty(reservation.ReservationPersonPhone) ||
                string.IsNullOrEmpty(reservation.ReservationForTimeIds) ||
                Guid.Empty == reservation.ReservationPlaceId)
            {
                msg = "预约信息不完整";
                return false;
            }
            if (IsReservationInfoInBlockList(reservation, out msg))
            {
                return false;
            }

            lock (_lock)
            {
                var reservationForDate = reservation.ReservationForDate;
                if (!IsReservationForDateAvailable(reservationForDate, isAdmin, out msg))
                {
                    return false;
                }
                if (!IsReservationForPeriodAvailable(reservationForDate, reservation.ReservationPlaceId, reservation.ReservationForTimeIds))
                {
                    msg = "预约时间段冲突，请重新选择预约时间段";
                    return false;
                }
                return true;
            }
        }
    }
}
