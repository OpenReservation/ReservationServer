using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Business
{
    public partial interface IBLLReservation : IBaseBLL<Reservation>
    {
        List<Reservation> GetReservationList<Tkey, TKey1>(int pageIndex, int pageSize, out int rowsCount,
            Expression<Func<Reservation, bool>> whereLambda, Expression<Func<Reservation, Tkey>> orderBy,
            Expression<Func<Reservation, TKey1>> orderby1, bool isAsc, bool isAsc1);
    }
}