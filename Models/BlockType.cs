using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tabBlockType")]
    public class BlockType
    {
        #region Private Field
        /// <summary>
        /// typeId
        /// </summary>
        private Guid typeId;

        /// <summary>
        /// typeName
        /// </summary>
        private string typeName; 
        #endregion

        /// <summary>
        /// 黑名单类型id
        /// </summary>
        [Column]
        [Key]
        public Guid TypeId
        {
            get
            {
                return typeId;
            }

            set
            {
                typeId = value;
            }
        }

        /// <summary>
        /// 黑名单类型名称
        /// </summary>
        [Column]
        public string TypeName
        {
            get
            {
                return typeName;
            }

            set
            {
                typeName = value;
            }
        }
    }
}