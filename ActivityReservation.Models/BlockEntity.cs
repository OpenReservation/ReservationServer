using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityReservation.Models
{
    [Table("tabBlockEntity")]
    public class BlockEntity
    {
        /// <summary>
        /// 黑名单id
        /// </summary>
        [Column]
        [Key]
        public Guid BlockId { get; set; }

        /// <summary>
        /// 黑名单类型id
        /// </summary>
        [Column]
        public Guid BlockTypeId { get; set; }

        /// <summary>
        /// 黑名单值
        /// </summary>
        [Column]
        public string BlockValue { get; set; }

        /// <summary>
        /// 添加到黑名单时间
        /// </summary>
        [Column]
        public DateTime BlockTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Column]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 黑名单类型信息
        /// </summary>
        [ForeignKey("BlockTypeId")]
        public virtual BlockType BlockType { get; set; }
    }
}
