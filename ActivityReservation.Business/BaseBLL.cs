using ActivityReservation.Database;
using WeihanLi.EntityFramework;

namespace ActivityReservation.Business
{
    public abstract class BaseBLL<T> : EFRepository<ReservationDbContext, T>, IBaseBLL<T> where T : class
    {
        public BaseBLL(ReservationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
