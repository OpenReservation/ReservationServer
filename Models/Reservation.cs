using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models
{
    [Table("tabReservation")]
    public class Reservation
    {
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
        /// 预约活动室名称
        /// </summary>
        private Guid reservationPlaceId;

        /// <summary>
        /// 预约时间
        /// </summary>
        private DateTime reservationTime = DateTime.Now;

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
        private int reservationStatus=0;

        /// <summary>
        /// 更新人
        /// </summary>
        private string updateBy;

        /// <summary>
        /// 更新时间
        /// </summary>
        private DateTime updateTime;

        /// <summary>
        /// 预约时间段1
        /// </summary>
        private bool t1 = false;
        /// <summary>
        ///  预约时间段2
        /// </summary>
        private bool t2 = false;
        /// <summary>
        ///  预约时间段3
        /// </summary>
        private bool t3 = false;
        /// <summary>
        ///  预约时间段4
        /// </summary>
        private bool t4 = false;
        /// <summary>
        ///  预约时间段5
        /// </summary>
        private bool t5 = false;
        /// <summary>
        ///  预约时间段6
        /// </summary>
        private bool t6 = false;
        /// <summary>
        ///  预约时间段7
        /// </summary>
        private bool t7 = false;

        [Column]
        [Key]
        public Guid ReservationId
        {
            get
            {
                return reservationId;
            }

            set
            {
                reservationId = value;
            }
        }

        [Column]
        public string ReservationPersonName
        {
            get
            {
                return reservationPersonName;
            }

            set
            {
                reservationPersonName = value;
            }
        }

        [Column]
        public string ReservationPersonPhone
        {
            get
            {
                return reservationPersonPhone;
            }

            set
            {
                reservationPersonPhone = value;
            }
        }

        [Column]
        [ForeignKey("Place")]
        public Guid ReservationPlaceId
        {
            get
            {
                return reservationPlaceId;
            }

            set
            {
                reservationPlaceId = value;
            }
        }

        public ReservationPlace Place { get; set; }

        [Column]
        public DateTime ReservationTime
        {
            get
            {
                return reservationTime;
            }

            set
            {
                reservationTime = value;
            }
        }
        [Column]
        public String ReservationForTime
        {
            get
            {
                return reservationForTime;
            }

            set
            {
                reservationForTime = value;
            }
        }

        [Column]
        public bool T1
        {
            get
            {
                return t1;
            }

            set
            {
                t1 = value;
            }
        }
        [Column]
        public bool T2
        {
            get
            {
                return t2;
            }

            set
            {
                t2 = value;
            }
        }
        [Column]
        public bool T3
        {
            get
            {
                return t3;
            }

            set
            {
                t3 = value;
            }
        }
        [Column]
        public bool T4
        {
            get
            {
                return t4;
            }

            set
            {
                t4 = value;
            }
        }
        [Column]
        public bool T5
        {
            get
            {
                return t5;
            }

            set
            {
                t5 = value;
            }
        }
        [Column]
        public bool T6
        {
            get
            {
                return t6;
            }

            set
            {
                t6 = value;
            }
        }
        [Column]
        public bool T7
        {
            get
            {
                return t7;
            }

            set
            {
                t7 = value;
            }
        }
        [Column]
        public int ReservationStatus
        {
            get
            {
                return reservationStatus;
            }

            set
            {
                reservationStatus = value;
            }
        }
        [Column]
        public string UpdateBy
        {
            get
            {
                return updateBy;
            }

            set
            {
                updateBy = value;
            }
        }
        [Column]
        public DateTime UpdateTime
        {
            get
            {
                return updateTime;
            }

            set
            {
                updateTime = value;
            }
        }
        [Column]
        public DateTime ReservationForDate
        {
            get
            {
                return reservationForDate;
            }

            set
            {
                reservationForDate = value;
            }
        }
    }
}