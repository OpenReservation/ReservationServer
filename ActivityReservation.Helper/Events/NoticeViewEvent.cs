using System;
using System.Threading.Tasks;
using ActivityReservation.Database;
using Microsoft.EntityFrameworkCore;
using WeihanLi.Common;
using WeihanLi.Common.Event;
using WeihanLi.Extensions;
using WeihanLi.Redis;

namespace ActivityReservation.Events
{
    public class NoticeViewEvent : EventBase
    {
        public Guid NoticeId { get; set; }

        // UserId
        // IP
        // ...
    }

    public class OnceEventHandlerBase
    {
        protected async Task<bool> IsHandleNeeded(EventBase @event)
        {
            var limiter = RedisManager.GetRateLimiterClient($"{@event.GetType().FullName}_{@event.EventId}",
                TimeSpan.FromMinutes(2));
            if (await limiter.AcquireAsync())
            {
                return true;
            }
            return false;
        }
    }

    public class NoticeViewEventHandler : OnceEventHandlerBase, IEventHandler<NoticeViewEvent>
    {
        public async Task Handle(NoticeViewEvent @event)
        {
            if (await IsHandleNeeded(@event))
            {
                await DependencyResolver.Current.TryInvokeServiceAsync<ReservationDbContext>(async dbContext =>
                {
                    //var notice = await dbContext.Notices.FindAsync(@event.NoticeId);
                    //notice.NoticeVisitCount += 1;
                    //await dbContext.SaveChangesAsync();

                    var conn = dbContext.Database.GetDbConnection();
                    await conn.ExecuteAsync($@"UPDATE tabNotice SET NoticeVisitCount = NoticeVisitCount +1 WHERE NoticeId = @NoticeId", new { @event.NoticeId });
                });
            }
        }
    }
}
