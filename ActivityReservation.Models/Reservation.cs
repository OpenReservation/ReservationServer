using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityReservation.Models
{
    [Table("tabReservation")]
    public class Reservation
    {
        #region Private Field

        /// <summary>
        /// 预约id
        /// </summary>
        private Guid reservationId;

        /// <summary>
        /// 预约人姓名
        /// </summary>
        private string reservationPersonName;

        /// <summary>
        /// 预约人联系方式
        /// </summary>
        private string reservationPersonPhone;

        /// <summary>
        /// 预约活动室id
        /// </summary>
        private Guid reservationPlaceId;

        /// <summary>
        /// 预约时间
        /// </summary>
        private DateTime reservationTime = DateTime.UtcNow;

        /// <summary>
        /// 预约使用的日期
        /// </summary>
        private DateTime reservationForDate;

        /// <summary>
        /// 预约使用时间
        /// </summary>
        private string reservationForTime;

        /// <summary>
        /// 预约状态
        /// 0：待审核
        /// 1：审核通过
        /// 2：审核不通过
        /// </summary>
        private int reservationStatus = 0;

        /// <summary>
        /// 预约人的IP
        /// </summary>
        private string reservationFromIp;

        /// <summary>
        /// 预约内容，活动内容
        /// </summary>
        private string reservationActivityContent;

        /// <summary>
        /// 预约单位
        /// </summary>
        private string reservationUnit;

        /// <summary>
        /// 更新/审核 备注信息
        /// </summary>
        private string updateMemo;

        /// <summary>
        /// 更新人
        /// </summary>
        private string updateBy;

        /// <summary>
        /// 更新时间
        /// </summary>
        private DateTime updateTime;

        #endregion Private Field

        /// <summary>
        /// 预约id
        /// </summary>
        [Column]
        [Key]
        public Guid ReservationId
        {
            get { return reservationId; }

            set { reservationId = value; }
        }

        /// <summary>
        /// 预约人姓名
        /// </summary>
        [Column]
        [Required]
        public string ReservationPersonName
        {
            get { return reservationPersonName; }

            set { reservationPersonName = value; }
        }

        /// <summary>
        /// 预约人联系方式
        /// </summary>
        [Column]
        [Required]
        public string ReservationPersonPhone
        {
            get { return reservationPersonPhone; }

            set { reservationPersonPhone = value; }
        }

        /// <summary>
        /// 预约时间
        /// </summary>
        [Column]
        public DateTime ReservationTime
        {
            get { return reservationTime; }

            set { reservationTime = value; }
        }

        /// <summary>
        /// 预约使用时间
        /// </summary>
        [Column]
        public String ReservationForTime
        {
            get { return reservationForTime; }

            set { reservationForTime = value; }
        }

        [Column]
        [Required]
        public string ReservationUnit
        {
            get { return reservationUnit; }
            set { reservationUnit = value; }
        }

        [Column]
        public string ReservationActivityContent
        {
            get { return reservationActivityContent; }
            set { reservationActivityContent = value; }
        }

        /// <summary>
        ///  预约时间段
        ///  按位与，这里取int
        ///  int 最多可以存 2^31，31个时间段，long 可以存63个时间段
        ///  uint 可以存 2^32个时间段，ulong 可以存64个时间段
        /// </summary>
        [Column]
        public int ReservationPeriod { get; set; }

        /// <summary>
        /// 预约状态
        /// 0：待审核
        /// 1：审核通过
        /// 2：审核不通过
        /// </summary>
        [Column]
        public int ReservationStatus
        {
            get { return reservationStatus; }

            set { reservationStatus = value; }
        }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column]
        public string UpdateBy
        {
            get { return updateBy; }

            set { updateBy = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column]
        public DateTime UpdateTime
        {
            get { return updateTime; }

            set { updateTime = value; }
        }

        /// <summary>
        /// 预约使用的日期
        /// </summary>
        [Column]
        public DateTime ReservationForDate
        {
            get { return reservationForDate; }

            set { reservationForDate = value; }
        }

        /// <summary>
        /// 预约人的IP
        /// </summary>
        [Column]
        public string ReservationFromIp
        {
            get { return reservationFromIp; }

            set { reservationFromIp = value; }
        }

        /// <summary>
        /// 预约活动室id
        /// </summary>
        [Column]
        public Guid ReservationPlaceId
        {
            get { return reservationPlaceId; }

            set { reservationPlaceId = value; }
        }

        /// <summary>
        /// 更新/审核 备注信息
        /// </summary>
        [Column]
        public string UpdateMemo
        {
            get { return updateMemo; }

            set { updateMemo = value; }
        }

        /// <summary>
        /// 预约活动室信息
        /// </summary>
        [ForeignKey("ReservationPlaceId")]
        public virtual ReservationPlace Place { get; set; }
    }
}
