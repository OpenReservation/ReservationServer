using System;
using System.Threading;
using System.Threading.Tasks;
using ActivityReservation.Database;
using ActivityReservation.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeihanLi.Common;
using WeihanLi.Common.Helpers;
using WeihanLi.EntityFramework;

namespace ActivityReservation.Services
{
    public abstract class ScheduedService : IHostedService, IDisposable
    {
        private readonly Timer _timer;
        private readonly TimeSpan _period;
        protected readonly ILogger Logger;

        protected ScheduedService(TimeSpan period, ILogger logger)
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

    public class RemoveOverduedReservtaionService : ScheduedService
    {
        public RemoveOverduedReservtaionService(ILogger<RemoveOverduedReservtaionService> logger) : base(TimeSpan.FromDays(1), logger)
        { }

        protected override Task ExecuteAsync()
        {
            return DependencyResolver.Current.TryInvokeServiceAsync<IEFRepository<ReservationDbContext, Reservation>>(reservationRepo =>
           {
               return reservationRepo.DeleteAsync(reservation => reservation.ReservationStatus == 0 && (reservation.ReservationForDate < DateTime.Today.AddDays(-3)));
           });
        }
    }
}
