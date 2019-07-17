using System;
using System.Threading;
using System.Threading.Tasks;
using ActivityReservation.Database;
using Microsoft.EntityFrameworkCore;
using WeihanLi.Common;
using WeihanLi.Common.Event;
using WeihanLi.Extensions;

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
        public Task Handle(NoticeViewEvent @event, CancellationToken cancellationToken = new CancellationToken())
        {
            return DependencyResolver.Current.TryInvokeServiceAsync<ReservationDbContext>(async dbContext =>
               {
                   //var notice = await dbContext.Notices.FindAsync(@event.NoticeId);
                   //notice.NoticeVisitCount += 1;
                   //await dbContext.SaveChangesAsync(cancellationToken);

                   var conn = dbContext.Database.GetDbConnection();
                   await conn.ExecuteAsync($@"UPDATE tabNotice SET NoticeVisitCount = NoticeVisitCount +1 WHERE NoticeId = @NoticeId", new { @event.NoticeId });
               });
        }
    }
}
