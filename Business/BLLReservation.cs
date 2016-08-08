using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Business
{
    public partial class BLLReservation
    {
        public List<Models.Reservation> GetReservationList<Tkey, TKey1>(int pageIndex, int pageSize, out int rowsCount, Expression<Func<Models.Reservation, bool>> whereLambda, Expression<Func<Models.Reservation, Tkey>> orderBy, Expression<Func<Models.Reservation, TKey1>> orderby1, bool isAsc, bool isAsc1)
        {
            return (dbHandler as DataAccess.DALReservation).GetReservationList(pageIndex, pageSize, out rowsCount, whereLambda, orderBy, orderby1, isAsc, isAsc1);
        }
    }
}