using Microsoft.EntityFrameworkCore;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Log;

namespace ActivityReservation.Models
{
    // Custom DbConfiguration
    //[DbConfigurationType(typeof(ReservationDbConfiguration))]
    public class ReservationDbContext : DbContext
    {
        private static readonly ILogHelper Logger = LogHelper.GetLogHelper<ReservationDbContext>();

        public ReservationDbContext(DbContextOptions options)
        {
            // Database.SetInitializer(new ReservationDbInitializer());
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<BlockType> BlockTypes { get; set; }
        public virtual DbSet<BlockEntity> BlockEntities { get; set; }
        public virtual DbSet<OperationLog> OperationLogs { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationPlace> ReservationPlaces { get; set; }
        public virtual DbSet<ReservationPeriod> ReservationPeriods { get; set; }
        public virtual DbSet<SystemSettings> SystemSettings { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }

        public virtual DbSet<DisabledPeriod> DisabledPeriods { get; set; }

        public virtual DbSet<WechatMenuConfig> WechatMenuConfigs { get; set; }
    }
}
