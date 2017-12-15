using DataAccess;
using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Business
{
    public partial class BLLReservation
    {
        public List<Reservation> GetReservationList<TKey, TKey1>(int pageIndex, int pageSize, out int rowsCount,
            Expression<Func<Reservation, bool>> whereLambda, Expression<Func<Reservation, TKey>> orderBy,
            Expression<Func<Reservation, TKey1>> orderby1, bool isAsc, bool isAsc1)
        {
            return ((IDALReservation)dbHandler).GetReservationList(pageIndex, pageSize, out rowsCount, whereLambda,
                orderBy, orderby1, isAsc, isAsc1);
        }
    }
}