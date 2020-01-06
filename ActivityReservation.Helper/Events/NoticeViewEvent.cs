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

    public abstract class OnceEventHandlerBase<TEvent> : EventHandlerBase<TEvent> where TEvent : class, IEventBase
    {
        protected async Task<bool> IsHandleNeeded(TEvent @event)
        {
            var limiter = RedisManager.GetRateLimiterClient($"{@event.GetType().FullName}_{@event.EventId}",
                TimeSpan.FromMinutes(2));
            return await limiter.AcquireAsync();
        }

        protected async Task<bool> Release(TEvent @event)
        {
            var limiter = RedisManager.GetRateLimiterClient($"{@event.GetType().FullName}_{@event.EventId}",
                TimeSpan.FromMinutes(2));
            return await limiter.ReleaseAsync();
        }
    }

    public class NoticeViewEventHandler : OnceEventHandlerBase<NoticeViewEvent>
    {
        public override async Task Handle(NoticeViewEvent @event)
        {
            if (await IsHandleNeeded(@event))
            {
                await DependencyResolver.Current.TryInvokeServiceAsync<ReservationDbContext>(async dbContext =>
                {
                    var notice = await dbContext.Notices.FindAsync(@event.NoticeId);
                    notice.NoticeVisitCount += 1;
                    await dbContext.SaveChangesAsync();

                    // var conn = dbContext.Database.GetDbConnection();
                    // await conn.ExecuteAsync($@"UPDATE tabNotice SET NoticeVisitCount = NoticeVisitCount +1 WHERE NoticeId = @NoticeId", new { @event.NoticeId });
                });
            }
        }
    }
}
