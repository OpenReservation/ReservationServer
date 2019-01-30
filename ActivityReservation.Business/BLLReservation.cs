using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ActivityReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityReservation.Business
{
    public partial class BLLReservation
    {
        public List<Reservation> GetReservationList<TKey, TKey1>(int pageIndex, int pageSize, out int rowsCount,
            Expression<Func<Reservation, bool>> whereLambda, Expression<Func<Reservation, TKey>> orderBy,
            Expression<Func<Reservation, TKey1>> orderby1, bool isAsc, bool isAsc1)
        {
            var count = Count(whereLambda);
            rowsCount = (int)count;
            if (count == 0)
            {
                return new List<Reservation>();
            }
            //
            var query = DbContext.Reservations.AsNoTracking()
                .Where(whereLambda);
            if (isAsc)
            {
                if (isAsc1)
                {
                    query = query.OrderBy(orderBy).ThenBy(orderby1);
                }
                else
                {
                    query = query.OrderBy(orderBy).ThenByDescending(orderby1);
                }
            }
            else
            {
                if (isAsc1)
                {
                    query = query.OrderByDescending(orderBy).ThenBy(orderby1);
                }
                else
                {
                    query = query.OrderByDescending(orderBy).ThenByDescending(orderby1);
                }
            }
            return query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(r => r.Place)
                .ToList();
        }
    }
}
