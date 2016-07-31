using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivityReservation.Helpers
{
    public class ListModel<T> where T:class,new()
    {
        public List<T> Data { get; set; }

        public PagerModel Pager { get; set; }
    }
}