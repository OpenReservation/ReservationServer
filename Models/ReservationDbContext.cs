using System.Data.Entity;

namespace Models
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext() : base("name=ReservationConn")
        {
            //DbInit
            Database.SetInitializer(new ReservationDbInitializer());
            //disable initializer
            //Database.SetInitializer<ReservationDbContext>(null);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<BlockType> BlockTypes { get; set; }
        public virtual DbSet<BlockEntity> BlockEntities { get; set; }
        public virtual DbSet<OperationLog> OperationLogs { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationPlace> ReservationPlaces { get; set; }
        public virtual DbSet<SystemSettings> SystemSettings { get; set; }
    }
}