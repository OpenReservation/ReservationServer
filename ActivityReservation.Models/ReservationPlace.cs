using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityReservation.Models
{
    [Table("tabReservationPlace")]
    public class ReservationPlace
    {
        /// <summary>
        /// 活动室id
        /// </summary>
        [Key]
        [Column]
        public Guid PlaceId { get; set; }

        /// <summary>
        /// 活动室名称
        /// </summary>
        [Column]
        [Required]
        [StringLength(16)]
        public string PlaceName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column]
        public int PlaceIndex { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新人
        /// </summary>
        [Column]
        [Required]
        public string UpdateBy { get; set; }

        /// <summary>
        /// 最多可预约时间段数量
        /// </summary>
        [Column]
        public int MaxReservationPeriodNum { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Column]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 是否删除
        /// </summary>
        [Column]
        public bool IsDel { get; set; }

        [NotMapped]
        public IReadOnlyList<ReservationPeriod> ReservationPeriodList { get; set; }
    }
}
