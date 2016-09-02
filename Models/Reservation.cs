using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
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

        /// <summary>
        /// 预约时间段1
        /// true:可预约，false:不可预约
        /// 预约之后某个时间段由true变为false
        /// </summary>
        private bool t1 = true;

        /// <summary>
        ///  预约时间段2
        /// </summary>
        private bool t2 = true;

        /// <summary>
        ///  预约时间段3
        /// </summary>
        private bool t3 = true;

        /// <summary>
        ///  预约时间段4
        /// </summary>
        private bool t4 = true;

        /// <summary>
        ///  预约时间段5
        /// </summary>
        private bool t5 = true;

        /// <summary>
        ///  预约时间段6
        /// </summary>
        private bool t6 = true;

        /// <summary>
        ///  预约时间段7
        /// </summary>
        private bool t7 = true; 
        #endregion

        /// <summary>
        /// 预约id
        /// </summary>
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

        /// <summary>
        /// 预约人姓名
        /// </summary>
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

        /// <summary>
        /// 预约人联系方式
        /// </summary>
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

        /// <summary>
        /// 预约时间
        /// </summary>
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

        /// <summary>
        /// 预约使用时间
        /// </summary>
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
        /// 预约时间段1
        /// true:可预约，false:不可预约
        /// 预约之后某个时间段由true变为false
        /// </summary>
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

        /// <summary>
        ///  预约时间段2
        /// </summary>
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

        /// <summary>
        ///  预约时间段3
        /// </summary>
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

        /// <summary>
        ///  预约时间段4
        /// </summary>
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

        /// <summary>
        ///  预约时间段5
        /// </summary>
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

        /// <summary>
        ///  预约时间段6
        /// </summary>
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

        /// <summary>
        ///  预约时间段7
        /// </summary>
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

        /// <summary>
        /// 预约状态
        /// 0：待审核
        /// 1：审核通过
        /// 2：审核不通过
        /// </summary>
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

        /// <summary>
        /// 更新人
        /// </summary>
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

        /// <summary>
        /// 更新时间
        /// </summary>
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

        /// <summary>
        /// 预约使用的日期
        /// </summary>
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

        /// <summary>
        /// 预约人的IP
        /// </summary>
        [Column]
        public string ReservationFromIp
        {
            get
            {
                return reservationFromIp;
            }

            set
            {
                reservationFromIp = value;
            }
        }

        /// <summary>
        /// 预约活动室id
        /// </summary>
        [Column]
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

        /// <summary>
        /// 更新/审核 备注信息
        /// </summary>
        [Column]
        public string UpdateMemo
        {
            get
            {
                return updateMemo;
            }

            set
            {
                updateMemo = value;
            }
        }

        /// <summary>
        /// 预约活动室信息
        /// </summary>
        [ForeignKey("ReservationPlaceId")]
        public virtual ReservationPlace Place { get; set; }        
    }
}