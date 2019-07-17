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

    public class NoticeViewEventHandler : IEventHandler<NoticeViewEvent>
    {
        public async Task Handle(NoticeViewEvent @event)
        {
            var firewallClient = RedisManager.GetFirewallClient($"{nameof(NoticeViewEventHandler)}_{@event.EventId}", TimeSpan.FromMinutes(5));
            if (await firewallClient.HitAsync())
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
