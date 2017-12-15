using System;
using System.ComponentModel.DataAnnotations;

namespace ActivityReservation.AdminLogic.ViewModels
{
    public class DisabledPeriodViewModel
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 每年重复
        /// </summary>
        [Required]
        public bool RepeatYearly { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        public bool IsModelValid()
        {
            return EndDate >= StartDate;
        }
    }
}