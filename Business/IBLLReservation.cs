using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Business
{
    public partial interface IBLLReservation : IBaseBLL<Models.Reservation>
    {
        List<Models.Reservation> GetReservationList<Tkey, TKey1>(int pageIndex, int pageSize, out int rowsCount, Expression<Func<Models.Reservation, bool>> whereLambda, Expression<Func<Models.Reservation, Tkey>> orderBy, Expression<Func<Models.Reservation, TKey1>> orderby1, bool isAsc, bool isAsc1);
    }
}