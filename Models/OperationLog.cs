using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tabOperationLog")]
    public class OperationLog
    {
        /// <summary>
        /// logId
        /// </summary>
        private Guid logId;

        /// <summary>
        /// operation time
        /// </summary>
        private DateTime operTime = DateTime.Now;

        /// <summary>
        /// logContent
        /// </summary>
        private string logContent;

        /// <summary>
        /// operation by
        /// </summary>
        private string operBy;

        [Key]
        [Column]
        public Guid LogId
        {
            get
            {
                return logId;
            }

            set
            {
                logId = value;
            }
        }

        [Column]
        public DateTime OperTime
        {
            get
            {
                return operTime;
            }

            set
            {
                operTime = value;
            }
        }

        [Column]
        public string LogContent
        {
            get
            {
                return logContent;
            }

            set
            {
                logContent = value;
            }
        }

        [Column]
        public string OperBy
        {
            get
            {
                return operBy;
            }

            set
            {
                operBy = value;
            }
        }
    }
}