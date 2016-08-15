using System.Collections.Generic;

namespace ActivityReservation.Helpers
{
    public class PagedListModel<T> where T : class, new()
    {
        public List<T> Data { get; set; }

        public PagerModel Pager { get; set; }
    }
}