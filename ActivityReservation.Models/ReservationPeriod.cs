using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityReservation.Models
{
    [Table("tabReservationPeriod")]
    public class ReservationPeriod
    {
        /// <summary>
        /// 主键，id
        /// </summary>
        [Key]
        [Column]
        public Guid PeriodId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Column]
        public string PeriodTitle { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column]
        public string PeriodDescription { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column]
        public int PeriodIndex { get; set; }

        /// <summary>
        /// 活动室id
        /// </summary>
        [Column]
        public Guid PlaceId { get; set; }

        /// <summary>
        /// CreateBy
        /// </summary>
        [Column]
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column]
        public string UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column]
        public DateTime UpdateTime { get; set; }

        [ForeignKey("PlaceId")]
        public ReservationPlace ReservationPlace { get; set; }
    }
}
