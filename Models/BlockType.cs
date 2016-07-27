using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tabBlockType")]
    public class BlockType
    {
        /// <summary>
        /// typeId
        /// </summary>
        private Guid typeId;

        /// <summary>
        /// typeName
        /// </summary>
        private string typeName;

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