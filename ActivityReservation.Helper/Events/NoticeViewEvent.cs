using System;
using System.Threading.Tasks;
using ActivityReservation.Database;
using WeihanLi.Common;
using WeihanLi.Common.Event;

namespace ActivityReservation.Events
{
    public class NoticeViewEvent : EventBase
    {
        public Guid NoticeId { get; set; }

        // UserId
        // IP
        // ...
    }

    public class NoticeViewEventHandler : IEventHandler<NoticeViewEvent>
    {
        public async Task Handle(NoticeViewEvent @event)
        {
            await DependencyResolver.Current.TryInvokeServiceAsync<ReservationDbContext>(async dbContext =>
            {
                var notice = await dbContext.Notices.FindAsync(@event.NoticeId);
                notice.NoticeVisitCount += 1;
                await dbContext.SaveChangesAsync();
            });
        }
    }
}
