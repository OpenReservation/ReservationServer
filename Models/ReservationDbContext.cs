using System.Data.Entity;
using WeihanLi.Common.Helpers;

namespace Models
{
    [DbConfigurationType(typeof(ReservationDbConfiguration))]
    public class ReservationDbContext : DbContext
    {
        private static readonly LogHelper Logger = LogHelper.GetLogHelper<ReservationDbContext>();

        public ReservationDbContext() : base("name=ReservationConn")
        {
            //log
            Database.Log = Logger.Debug;

            Database.SetInitializer(new ReservationDbInitializer());
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<BlockType> BlockTypes { get; set; }
        public virtual DbSet<BlockEntity> BlockEntities { get; set; }
        public virtual DbSet<OperationLog> OperationLogs { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationPlace> ReservationPlaces { get; set; }
        public virtual DbSet<SystemSettings> SystemSettings { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }

        public virtual DbSet<DisabledPeriod> DisabledPeriods { get; set; }
    }
}