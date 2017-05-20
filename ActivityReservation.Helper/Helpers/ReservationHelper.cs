using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivityReservation.Helpers
{
    public class ReservationHelper
    {
        /// <summary>
        /// 根据预约日期和预约地点获取可用的预约时间段
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <param name="placeId">预约地点id</param>
        /// <returns></returns>
        public static bool[] GetAvailabelPeriodsByDateAndPlace(DateTime dt, Guid placeId)
        {
            //待审核和审核通过的预约时间段不能再被预约
            List<Models.Reservation> reservationList = new Business.BLLReservation().GetAll(r => System.Data.Entity.DbFunctions.DiffDays(r.ReservationForDate, dt) == 0 && r.ReservationPlaceId == placeId && r.ReservationStatus != 2);
            bool[] periodsStatus = new bool[7] { true, true, true, true, true, true, true };
            foreach (Models.Reservation item in reservationList)
            {
                if (periodsStatus[0] && !item.T1)
                {
                    periodsStatus[0] = false;                    
                }
                if (periodsStatus[1] && !item.T2)
                {
                    periodsStatus[1] = false;                    
                }
                if (periodsStatus[2] && !item.T3)
                {
                    periodsStatus[2] = false;
                }
                if (periodsStatus[3] && !item.T4)
                {
                    periodsStatus[3] = false;
                }
                if (periodsStatus[4] && !item.T5)
                {
                    periodsStatus[4] = false;                    
                }
                if (periodsStatus[5] && !item.T6)
                {
                    periodsStatus[5] = false;
                }
                if (periodsStatus[6] && !item.T7)
                {
                    periodsStatus[6] = false;
                }
            }
            return periodsStatus;
        }

        /// <summary>
        /// 判断预约日期是否在可预约范围内
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <returns></returns>
        private static bool IsReservationForDateAvailabel(DateTime dt)
        {
            int daysdiff = dt.Subtract(DateTime.Today).Days;
            if (daysdiff>=0&&daysdiff<=7)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断预约日期是否在禁用时间段范围内
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <returns></returns>
        private static bool IsReservationForDateDisabled(DateTime dt)
        {
            var disabledPeriods = new Business.BLLDisabledPeriod().GetAll(p=>!p.IsDeleted && p.IsActive && (System.Data.Entity.DbFunctions.DiffDays(p.StartDate, dt) >= 0 && System.Data.Entity.DbFunctions.DiffDays(p.StartDate, dt) <= 0));
            if (disabledPeriods != null && disabledPeriods.Any())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断预约时间段是否可用
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <param name="placeId">预约地点id</param>
        /// <param name="reservationForPeriodIds">预约时间段id</param>
        /// <returns></returns>
        private static bool IsReservationForPeriodAvailable(DateTime dt, Guid placeId,string reservationForPeriodIds)
        {
            List<Models.Reservation> reservationList = new Business.BLLReservation().GetAll(r => System.Data.Entity.DbFunctions.DiffDays(r.ReservationForDate, dt) == 0 && r.ReservationPlaceId == placeId && r.ReservationStatus != 2);
            bool[] periodsStatus = new bool[7] { true, true, true, true, true, true, true };
            foreach (Models.Reservation item in reservationList)
            {
                if (periodsStatus[0] && !item.T1)
                {
                    periodsStatus[0] = false;
                }
                if (periodsStatus[1] && !item.T2)
                {
                    periodsStatus[1] = false;
                }
                if (periodsStatus[2] && !item.T3)
                {
                    periodsStatus[2] = false;
                }
                if (periodsStatus[3] && !item.T4)
                {
                    periodsStatus[3] = false;
                }
                if (periodsStatus[4] && !item.T5)
                {
                    periodsStatus[4] = false;
                }
                if (periodsStatus[5] && !item.T6)
                {
                    periodsStatus[5] = false;
                }
                if (periodsStatus[6] && !item.T7)
                {
                    periodsStatus[6] = false;
                }
            }
            string[] periodIds = reservationForPeriodIds.Split(',');
            //
            foreach (string item in periodIds)
            {
                int index = Convert.ToInt32(item);
                if (!periodsStatus[index - 1])
                {
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// 提交信息是否在黑名单中
        /// </summary>
        /// <param name="reservation">预约信息</param>
        /// <param name="mesage">错误信息</param>
        /// <returns></returns>
        private static bool IsReservationInfoInBlockList(ViewModels.ReservationViewModel reservation, out string mesage)
        {
            List<Models.BlockEntity> blockList = new Business.BLLBlockEntity().GetAll(b => b.IsActive);
            mesage = "";
            //预约人手机号
            if (blockList.Any(b => b.BlockValue.Equals(reservation.ReservationPersonPhone)))
            {
                mesage = "手机号已被拉黑";
                return true;
            }
            //预约人IP地址
            string ip = HttpContext.Current.Request.UserHostAddress;
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
        public static bool IsReservationAvailabel(ViewModels.ReservationViewModel reservation, out string msg,bool isAdmin = false)
        {
            if (reservation == null || String.IsNullOrEmpty(reservation.ReservationPersonName) || string.IsNullOrEmpty(reservation.ReservationPersonPhone) || string.IsNullOrEmpty(reservation.ReservationForTimeIds) || Guid.Empty == reservation.ReservationPlaceId)
            {
                msg = "预约信息为空";
                return false;
            }
            DateTime reservationForDate = reservation.ReservationForDate;
            if (!isAdmin)
            {
                if (!IsReservationForDateAvailabel(reservationForDate))
                {
                    msg = "预约日期不在可预约范围内";
                    return false;
                }
            }
            if (IsReservationForDateDisabled(reservationForDate))
            {
                msg = "预约日期已被禁用，不可预约，如要预约请联系网站管理员";
                return false;
            }
            if (!IsReservationForPeriodAvailable(reservationForDate,reservation.ReservationPlaceId,reservation.ReservationForTimeIds))
            {
                msg = "预约时间段冲突，请重新选择预约时间段";
                return false;
            }
            return !IsReservationInfoInBlockList(reservation, out msg);
        }
    }
}