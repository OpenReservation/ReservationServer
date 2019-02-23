using ActivityReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivityReservation.Database
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions options) : base(options)
        {
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
