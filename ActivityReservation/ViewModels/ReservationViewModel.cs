using System;
using System.ComponentModel.DataAnnotations;

namespace ActivityReservation.ViewModels
{
    public class ReservationViewModel
    {
        [Required]
        public DateTime ReservationForDate { get; set; }
        [Required]
        public string ReservationForTime { get; set; }
        [Required]
        public string ReservationForTimeIds { get; set; }
        [Required]
        public string ReservationPlaceName { get; set; }
        [Required]
        public Guid ReservationPlaceId { get; set; }
        [Required]
        [StringLength(4,MinimumLength =2,ErrorMessage ="联系人姓名不合法")]
        public string ReservationPersonName { get; set; }
        [Required]
        [RegularExpression("/d{11}",ErrorMessage ="联系方式不合法")]
        public string ReservationPersonPhone { get; set; }
    }
}