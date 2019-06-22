using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using WeihanLi.Common.Json;

namespace ActivityReservation.ViewModels
{
    public class ReservationListViewModel
    {
        [Display(Name = "预约日期")]
        [JsonConverter(typeof(DateTimeFormatConverter), "yyyy-MM-dd")]
        public DateTime ReservationForDate { get; set; }

        [Display(Name = "预约时间段")]
        public string ReservationForTime { get; set; }

        public string ReservationPersonPhone { get; set; }

        public string ReservationPersonName { get; set; }

        public string ReservationUnit { get; set; }

        public string ReservationPlaceName { get; set; }

        [Display(Name = "预约活动内容")]
        public string ReservationActivityContent { get; set; }

        public Guid ReservationId { get; set; }

        public DateTime ReservationTime { get; set; }

        public int ReservationStatus { get; set; }
    }

    public class ReservationViewModel
    {
        [Required]
        [Display(Name = "预约日期")]
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
        [Display(Name = "预约活动内容")]
        public string ReservationActivityContent { get; set; }

        [Required]
        [Display(Name = "预约人姓名")]
        [StringLength(16, MinimumLength = 2, ErrorMessage = "联系人姓名不合法")]
        public string ReservationPersonName { get; set; }

        [Required]
        [Display(Name = "预约人联系方式")]
        [RegularExpression(@"^1\d{10}$", ErrorMessage = "联系方式不合法")]
        public string ReservationPersonPhone { get; set; }
    }
}
