using System.Threading.Tasks;
using OpenReservation.Database;
using OpenReservation.Events;
using WeihanLi.Common;
using WeihanLi.Common.Event;

namespace OpenReservation.API.Test.MockServices
{
    internal class MockNoticeViewEventHandler : EventHandlerBase<NoticeViewEvent>
    {
        public override async Task Handle(NoticeViewEvent @event)
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
