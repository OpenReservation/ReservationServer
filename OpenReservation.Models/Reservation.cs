using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenReservation.Models
{
    [Table("tabReservation")]
    public class Reservation
    {
        /// <summary>
        /// 预约id
        /// </summary>
        [Column]
        [Key]
        public Guid ReservationId { get; set; }

        /// <summary>
        /// 预约人姓名
        /// </summary>
        [Column]
        [StringLength(16)]
        [Required]
        public string ReservationPersonName { get; set; }

        /// <summary>
        /// 预约人联系方式
        /// </summary>
        [Column]
        [StringLength(16)]
        [Required]
        public string ReservationPersonPhone { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        [Column]
        public DateTime ReservationTime { get; set; }

        /// <summary>
        /// 预约使用时间
        /// </summary>
        [Column]
        public string ReservationForTime { get; set; }

        /// <summary>
        /// 预约单位
        /// </summary>
        [Column]
        [Required]
        public string ReservationUnit { get; set; }

        /// <summary>
        /// 预约内容，活动内容
        /// </summary>
        [Column]
        public string ReservationActivityContent { get; set; }

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
        /// 3: 已取消
        /// </summary>
        [Column]
        public ReservationStatus ReservationStatus { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public Guid ReservedBy { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [Column]
        public string UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 预约使用的日期
        /// </summary>
        [Column]
        public DateTime ReservationForDate { get; set; }

        /// <summary>
        /// 预约人的IP
        /// </summary>
        [Column]
        public string ReservationFromIp { get; set; }

        /// <summary>
        /// 预约活动室id
        /// </summary>
        [Column]
        public Guid ReservationPlaceId { get; set; }

        /// <summary>
        /// 更新/审核 备注信息
        /// </summary>
        [Column]
        public string UpdateMemo { get; set; }

        /// <summary>
        /// 预约活动室信息
        /// </summary>
        [ForeignKey("ReservationPlaceId")]
        public virtual ReservationPlace Place { get; set; }
    }

    public enum ReservationStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        [Description("待审核")]
        UnReviewed = 0,

        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        Reviewed = 1,

        /// <summary>
        /// 被拒绝
        /// </summary>
        [Description("未通过审核")]
        Rejected = 2,

        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Deleted = 3,

        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Canceled = 4,
    }
}
