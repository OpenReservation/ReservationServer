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
        
        public BlockType BlockType { get; set; }
    }
}