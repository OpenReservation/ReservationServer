using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tabBlockEntity")]
    public class BlockEntity
    {
        #region Private Field

        /// <summary>
        /// id
        /// </summary>
        private Guid blockId;

        /// <summary>
        /// blockTypeId
        /// </summary>
        private Guid blockTypeId;

        /// <summary>
        /// blockValue
        /// </summary>
        private string blockValue;

        /// <summary>
        /// block time
        /// </summary>
        private DateTime blockTime;

        /// <summary>
        /// 是否启用
        /// </summary>
        private bool isActive = true;

        #endregion Private Field

        /// <summary>
        /// 黑名单id
        /// </summary>
        [Column]
        [Key]
        public Guid BlockId
        {
            get { return blockId; }

            set { blockId = value; }
        }

        /// <summary>
        /// 黑名单类型id
        /// </summary>
        [Column]
        public Guid BlockTypeId
        {
            get { return blockTypeId; }

            set { blockTypeId = value; }
        }

        /// <summary>
        /// 黑名单值
        /// </summary>
        [Column]
        public string BlockValue
        {
            get { return blockValue; }

            set { blockValue = value; }
        }

        /// <summary>
        /// 添加到黑名单时间
        /// </summary>
        [Column]
        public DateTime BlockTime
        {
            get { return blockTime; }

            set { blockTime = value; }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Column]
        public bool IsActive
        {
            get { return isActive; }

            set { isActive = value; }
        }

        /// <summary>
        /// 黑名单类型信息
        /// </summary>
        [ForeignKey("BlockTypeId")]
        public virtual BlockType BlockType { get; set; }
    }
}