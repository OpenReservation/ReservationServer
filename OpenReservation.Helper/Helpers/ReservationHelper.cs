using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OpenReservation.Business;
using OpenReservation.Models;
using OpenReservation.ViewModels;
using WeihanLi.Common;
using WeihanLi.Extensions;
using WeihanLi.Redis;
using WeihanLi.Web.Extensions;

namespace OpenReservation.Helpers
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
            var reservationList = _bllReservation.Select(r =>
                r.ReservationForDate == dt
                && r.ReservationPlaceId == placeId
                && r.ReservationStatus != ReservationStatus.Rejected
                && r.ReservationStatus != ReservationStatus.Canceled
                );

            var reservationPeriod = _bllReservationPeriod
                .Select(_ => _.PlaceId == placeId)
                .OrderBy(_ => _.PeriodIndex)
                .ThenBy(_ => _.CreateTime);

            return reservationPeriod.Select((_, index) => new ReservationPeriodViewModel
            {
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
            // 不可以预约之前的日期
            var daysDiff = dt.Subtract(DateTime.UtcNow.AddHours(8).Date).Days;
            if (daysDiff < 0)
            {
                msg = "预约日期不可预约";
                return false;
            }

            // 最大预约天数（后面可以改成根据活动室去配置
            if (!isAdmin && daysDiff > MaxReservationDiffDays)
            {
                msg = $"预约日期需要在{MaxReservationDiffDays}天内";
                return false;
            }

            if (!_bllDisabledPeriod.Any(builder => builder.WithPredict(p => p.IsActive
                && p.StartDate >= dt
                && dt >= p.EndDate
                )))
            {
                msg = string.Empty;
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
            // 根据活动室配置最大可预约的时间段数量

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
            var blockList = RedisManager.CacheClient.GetOrSet(Constants.BlackListCacheKey,
                () => _bllBlockEntity.Select(_ => _.IsActive),
                TimeSpan.FromDays(1));

            message = string.Empty;
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

        /// <summary>
        /// 新建预约
        /// </summary>
        /// <param name="reservation">预约信息</param>
        /// <param name="msg">预约错误提示信息</param>
        /// <param name="isAdmin">是否是管理员预约</param>
        /// <returns></returns>
        public bool MakeReservation(ReservationViewModel reservation, out string msg,
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
            var reservationForDate = reservation.ReservationForDate;
            if (!IsReservationForDateAvailable(reservationForDate, isAdmin, out msg))
            {
                return false;
            }

            using (var redisLock = RedisManager.GetRedLockClient($"reservation:{reservation.ReservationPlaceId:N}:{reservation.ReservationForDate:yyyyMMdd}"))
            {
                if (redisLock.TryLock())
                {
                    if (!IsReservationForPeriodAvailable(reservationForDate, reservation.ReservationPlaceId, reservation.ReservationForTimeIds))
                    {
                        msg = "预约时间段冲突，请重新选择预约时间段";
                        return false;
                    }

                    var context = DependencyResolver.ResolveService<IHttpContextAccessor>().HttpContext;
                    var userId = context.User.GetUserId<Guid>();

                    if (!isAdmin)
                    {
                        if (_bllReservation.Count(r =>
                            r.ReservedBy == userId &&
                            r.ReservationTime.AddHours(8).Date == DateTime.UtcNow.AddHours(8).Date
                        ) >= 3)
                        {
                            msg = "今日预约已达上限，请明日再约";
                            return false;
                        }
                    }

                    var reservationEntity = new Reservation()
                    {
                        ReservationForDate = reservation.ReservationForDate,
                        ReservationForTime = reservation.ReservationForTime,
                        ReservationPlaceId = reservation.ReservationPlaceId,

                        ReservationUnit = reservation.ReservationUnit,
                        ReservationActivityContent = reservation.ReservationActivityContent,
                        ReservationPersonName = reservation.ReservationPersonName,
                        ReservationPersonPhone = reservation.ReservationPersonPhone,
                        ReservationFromIp = context.GetUserIP(),

                        ReservedBy = userId,

                        UpdateBy = reservation.ReservationPersonName,
                        ReservationTime = DateTime.UtcNow,
                        UpdateTime = DateTime.UtcNow,
                        ReservationId = Guid.NewGuid()
                    };
                    //验证最大可预约时间段，同一个手机号，同一个IP地址
                    foreach (var item in reservation.ReservationForTimeIds.SplitArray<int>())
                    {
                        reservationEntity.ReservationPeriod += (1 << item);
                    }
                    if (isAdmin)
                    {
                        reservationEntity.ReservationStatus = ReservationStatus.Reviewed;
                    }
                    _bllReservation.Insert(reservationEntity);

                    return true;
                }
                else
                {
                    msg = "系统繁忙，请稍后重试！";
                    return false;
                }
            }
        }
    }
}
