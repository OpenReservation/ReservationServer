using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityReservation.Models
{
    /// <summary>
    /// 禁用时间段
    /// </summary>
    [Table("tabDisabledPeriod")]
    public class DisabledPeriod
    {
        #region private field

        private Guid periodId;
        private DateTime startDate;
        private DateTime endDate;
        private bool repeatYearly;
        private bool isActive;
        private string updatedBy;
        private DateTime updatedTime;
        private bool isDeleted;

        #endregion private field

        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid PeriodId
        {
            get { return periodId; }

            set { periodId = value; }
        }

        /// <summary>
        /// 开始日期
        /// </summary>
        [Column]
        public DateTime StartDate
        {
            get { return startDate; }

            set { startDate = value; }
        }

        /// <summary>
        /// 结束日期
        /// </summary>
        [Column]
        public DateTime EndDate
        {
            get { return endDate; }

            set { endDate = value; }
        }

        /// <summary>
        /// 每年都禁用
        /// </summary>
        [Column]
        public bool RepeatYearly
        {
            get { return repeatYearly; }

            set { repeatYearly = value; }
        }

        /// <summary>
        /// 更新人
        /// </summary>
        [Column]
        public string UpdatedBy
        {
            get { return updatedBy; }

            set { updatedBy = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column]
        public DateTime UpdatedTime
        {
            get { return updatedTime; }

            set { updatedTime = value; }
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
        /// 是否删除
        /// </summary>
        [Column]
        public bool IsDeleted
        {
            get { return isDeleted; }

            set { isDeleted = value; }
        }
    }
}
