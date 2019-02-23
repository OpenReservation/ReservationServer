using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ActivityReservation.Models;

namespace ActivityReservation.Business
{
    public partial interface IBLLReservation
    {
        List<Reservation> GetReservationList<Tkey, TKey1>(int pageIndex, int pageSize, out int rowsCount,
            Expression<Func<Reservation, bool>> whereLambda, Expression<Func<Reservation, Tkey>> orderBy,
            Expression<Func<Reservation, TKey1>> orderby1, bool isAsc, bool isAsc1);
    }
}
