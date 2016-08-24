using System;
using System.ComponentModel.DataAnnotations;

namespace ActivityReservation.ViewModels
{
    public class ReservationViewModel
    {
        [Required]
        [Display(Name ="预约日期")]
        public DateTime ReservationForDate { get; set; }
        [Required]
        [Display(Name = "预约时间段")]
        public string ReservationForTime { get; set; }
        [Required]
        [Display(Name = "预约日期段ids")]
        public string ReservationForTimeIds { get; set; }
        [Required]
        [Display(Name = "预约活动室名称")]
        public string ReservationPlaceName { get; set; }
        [Required]
        [Display(Name = "预约活动室id")]
        public Guid ReservationPlaceId { get; set; }
        [Required]
        [Display(Name = "预约单位")]
        public string ReservationUnit { get; set; }
        [Required]
        [Display(Name ="预约活动内容")]
        public string ReservationActivityContent { get; set; }
        [Required]
        [Display(Name ="预约人姓名")]
        [StringLength(4,MinimumLength =2,ErrorMessage ="联系人姓名不合法")]
        public string ReservationPersonName { get; set; }
        [Required]
        [Display(Name = "预约人联系方式")]
        [RegularExpression(@"^\d{11}$",ErrorMessage ="联系方式不合法")]
        public string ReservationPersonPhone { get; set; }
    }
}