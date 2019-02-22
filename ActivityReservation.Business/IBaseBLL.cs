using ActivityReservation.Database;
using WeihanLi.EntityFramework;

namespace ActivityReservation.Business
{
    public interface IBaseBLL<T> : IEFRepository<ReservationDbContext, T> where T : class
    {
    }
}
