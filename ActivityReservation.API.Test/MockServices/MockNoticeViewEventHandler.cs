using System.Threading.Tasks;
using ActivityReservation.Database;
using ActivityReservation.Events;
using WeihanLi.Common;
using WeihanLi.Common.Event;

namespace ActivityReservation.API.Test.MockServices
{
    internal class MockNoticeViewEventHandler : IEventHandler<NoticeViewEvent>
    {
        public async Task Handle(NoticeViewEvent @event)
        {
            await DependencyResolver.Current.TryInvokeServiceAsync<ReservationDbContext>(async dbContext =>
            {
                var notice = await dbContext.Notices.FindAsync(@event.NoticeId);
                if (null != notice)
                {
                    notice.NoticeVisitCount += 1;
                    await dbContext.SaveChangesAsync();
                }
            });
        }
    }
}
