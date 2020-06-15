using System;
using OpenReservation.Models;
using Microsoft.EntityFrameworkCore;
using WeihanLi.EntityFramework.Audit;

namespace OpenReservation.Database
{
    public class ReservationDbContext : AuditDbContext
    {
        public ReservationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // globalFilters
            modelBuilder.Entity<ReservationPlace>().HasQueryFilter(x => !x.IsDel);
            modelBuilder.Entity<ReservationPeriod>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Notice>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Reservation>().HasQueryFilter(r => r.ReservationStatus != ReservationStatus.Deleted);

            modelBuilder.Entity<AuditRecord>()
                .ToTable("AuditRecords");

            if (!Database.ProviderName.EndsWith("InMemory", StringComparison.OrdinalIgnoreCase))
            {
                modelBuilder.Entity<Notice>().HasIndex(x => x.NoticeCustomPath); // path
            }
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
    }
}
