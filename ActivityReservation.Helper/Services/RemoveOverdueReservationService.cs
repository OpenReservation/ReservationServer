using System;
using System.Threading;
using System.Threading.Tasks;
using ActivityReservation.Database;
using ActivityReservation.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeihanLi.Common.Helpers;
using WeihanLi.EntityFramework;
using WeihanLi.Redis;

namespace ActivityReservation.Services
{
    public abstract class ScheduledService : IHostedService, IDisposable
    {
        private readonly Timer _timer;
        private readonly TimeSpan _period;
        protected readonly ILogger Logger;

        protected ScheduledService(TimeSpan period, ILogger logger)
        {
            Logger = logger;
            _period = period;
            _timer = new Timer(Execute, null, Timeout.Infinite, 0);
        }

        public void Execute(object state = null)
        {
            try
            {
                Logger.LogInformation("Begin execute service");
                ExecuteAsync().Wait();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Execute exception");
            }
            finally
            {
                Logger.LogInformation("Execute finished");
            }
        }

        protected abstract Task ExecuteAsync();

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Service is starting.");
            _timer.Change(TimeSpan.FromSeconds(SecurityHelper.Random.Next(10)), _period);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }

    public class RemoveOverdueReservationService : ScheduledService
    {
        private readonly IServiceProvider _serviceProvider;

        public RemoveOverdueReservationService(ILogger<RemoveOverdueReservationService> logger,
            IServiceProvider serviceProvider) : base(TimeSpan.FromDays(1), logger)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync()
        {
            var firewall = RedisManager.GetFirewallClient($"{nameof(RemoveOverdueReservationService)}_firewall", TimeSpan.FromMinutes(1));
            if (await firewall.HitAsync())
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var reservationRepo = scope.ServiceProvider.GetRequiredService<IEFRepository<ReservationDbContext, Reservation>>();
                    await reservationRepo.DeleteAsync(reservation => reservation.ReservationStatus == 0 && (reservation.ReservationForDate < DateTime.Today.AddDays(-3)));
                }
            }
        }
    }
}
