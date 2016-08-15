using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tabBlockEntity")]
    public class BlockEntity
    {
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

        [Column]
        [Key]
        public Guid BlockId
        {
            get
            {
                return blockId;
            }

            set
            {
                blockId = value;
            }
        }

        [Column]
        [ForeignKey("BlockType")]
        public Guid BlockTypeId
        {
            get
            {
                return blockTypeId;
            }

            set
            {
                blockTypeId = value;
            }
        }

        [Column]
        public string BlockValue
        {
            get
            {
                return blockValue;
            }

            set
            {
                blockValue = value;
            }
        }
        [Column]
        public DateTime BlockTime
        {
            get
            {
                return blockTime;
            }

            set
            {
                blockTime = value;
            }
        }

        [Column]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
            }
        }

        public virtual BlockType BlockType { get; set; }
    }
}