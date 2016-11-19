using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivityReservation.Helpers
{
    public class ReservationHelper
    {
        /// <summary>
        /// 提交信息是否在黑名单中
        /// </summary>
        /// <param name="reservation">预约信息</param>
        /// <returns></returns>
        public static bool IsInBlockList(ViewModels.ReservationViewModel reservation)
        {
            List<Models.BlockEntity> blockList = new Business.BLLBlockEntity().GetAll(b=>b.IsActive);
            //预约人手机号
            if (blockList.Any(b => b.BlockValue.Equals(reservation.ReservationPersonPhone)))
            {
                return true;
            }
            //预约人IP地址
            string ip = HttpContext.Current.Request.UserHostAddress;
            if (blockList.Any(b => b.BlockValue.Equals(ip)))
            {
                return true;
            }
            //预约人姓名
            if (blockList.Any(b => b.BlockValue.Equals(reservation.ReservationPersonName)))
            {
                return true;
            }
            return false;
        }

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
                if (periodsStatus[0])
                {
                    if (!item.T1)
                    {
                        periodsStatus[0] = false;
                    }
                }
                if (periodsStatus[1])
                {
                    if (!item.T2)
                    {
                        periodsStatus[1] = false;
                    }
                }
                if (periodsStatus[2])
                {
                    if (!item.T3)
                    {
                        periodsStatus[2] = false;
                    }
                }
                if (periodsStatus[3])
                {
                    if (!item.T4)
                    {
                        periodsStatus[3] = false;
                    }
                }
                if (periodsStatus[4])
                {
                    if (!item.T5)
                    {
                        periodsStatus[4] = false;
                    }
                }
                if (periodsStatus[5])
                {
                    if (!item.T6)
                    {
                        periodsStatus[5] = false;
                    }
                }
                if (periodsStatus[6])
                {
                    if (!item.T7)
                    {
                        periodsStatus[6] = false;
                    }
                }
            }
            return periodsStatus;
        }

        /// <summary>
        /// 判断预约日期是否在可预约范围内
        /// </summary>
        /// <param name="dt">预约日期</param>
        /// <returns></returns>
        public static bool IsReservationForDateAvailabel(DateTime dt)
        {
            int daysdiff = dt.Subtract(DateTime.Today).Days;
            if (daysdiff>=0&&daysdiff<=7)
            {
                return true;
            }
            return false;
        }
    }
}