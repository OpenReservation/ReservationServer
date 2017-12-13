using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess;

namespace Business
{
    public partial class BLLReservation
    {
        public List<Models.Reservation> GetReservationList<TKey, TKey1>(int pageIndex, int pageSize, out int rowsCount, Expression<Func<Models.Reservation, bool>> whereLambda, Expression<Func<Models.Reservation, TKey>> orderBy, Expression<Func<Models.Reservation, TKey1>> orderby1, bool isAsc, bool isAsc1)
        {
            return ((IDALReservation)dbHandler).GetReservationList(pageIndex, pageSize, out rowsCount, whereLambda, orderBy, orderby1, isAsc, isAsc1);
        }
    }
}