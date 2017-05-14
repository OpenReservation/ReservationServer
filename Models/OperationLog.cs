using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tabOperationLog")]
    public class OperationLog
    {
        #region Private Field
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

        private string ipAddress;

        /// <summary>
        /// operation by
        /// </summary>
        private string operBy;

        /// <summary>
        /// logModule
        /// </summary>
        private string logModule; 
        #endregion

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

        /// <summary>
        /// 操作时间
        /// </summary>
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

        /// <summary>
        /// 操作描述
        /// </summary>
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

        /// <summary>
        /// 操作IP
        /// </summary>
        [Column]
        public string IpAddress
        {
            get
            {
                return ipAddress;
            }

            set
            {
                ipAddress = value;
            }
        }

        /// <summary>
        /// 操作人
        /// </summary>
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

        /// <summary>
        /// 日志模块
        /// </summary>
        [Column]
        public string LogModule
        {
            get
            {
                return logModule;
            }

            set
            {
                logModule = value;
            }
        }

    }
}