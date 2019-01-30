using WeihanLi.EntityFramework;

namespace ActivityReservation.Business
{
    public interface IBaseBLL<T> : IEFRepository<T> where T : class
    {
    }
}
