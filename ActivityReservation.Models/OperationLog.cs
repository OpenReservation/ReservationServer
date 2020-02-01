using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityReservation.Models
{
    [Table("tabOperationLog")]
    public class OperationLog
    {
        [Key]
        [Column]
        public Guid LogId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Column]
        public DateTime OperTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 操作描述
        /// </summary>
        [Column]
        public string LogContent { get; set; }

        /// <summary>
        /// 操作IP
        /// </summary>
        [Column]
        public string IpAddress { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [Column]
        [Required]
        public string OperBy { get; set; }

        /// <summary>
        /// 日志模块
        /// </summary>
        [Column]
        [Required]
        [StringLength(32)]
        public string LogModule { get; set; }
    }
}
