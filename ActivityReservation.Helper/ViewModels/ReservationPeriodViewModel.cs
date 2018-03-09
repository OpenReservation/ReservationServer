using System;

namespace ActivityReservation.ViewModels
{
    public class ReservationPeriodViewModel
    {
        public int PeriodIdx { get; set; }

        public Guid PeriodId { get; set; }
        public int PeriodIndex { get; set; }
        public string PeriodTitle { get; set; }
        public string PeriodDescription { get; set; }

        public bool IsCanReservate { get; set; }
    }
}
