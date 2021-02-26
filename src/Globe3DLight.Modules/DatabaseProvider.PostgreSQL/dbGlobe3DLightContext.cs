using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

//#nullable disable

namespace Globe3DLight.DatabaseProvider.PostgreSQL
{
    //public class AppContext : DbContext
    //{
    //    private readonly string _connectionString;

    //    public AppContext(string connectionString)
    //    {
    //        _connectionString = connectionString;
    //    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseNpgsql(_connectionString);
    //    }
    //}


    internal partial class dbGlobe3DLightContext : DbContext
    {
        public dbGlobe3DLightContext()
        {
        }

        public dbGlobe3DLightContext(DbContextOptions<dbGlobe3DLightContext> options)
            : base(options)
        {
        }

        public virtual DbSet<GroundObject> GroundObjects { get; set; }
        public virtual DbSet<GroundStation> GroundStations { get; set; }
        public virtual DbSet<InitialCondition> InitialConditions { get; set; }
        public virtual DbSet<Retranslator> Retranslators { get; set; }
        public virtual DbSet<RetranslatorPosition> RetranslatorPositions { get; set; }
        public virtual DbSet<Satellite> Satellites { get; set; }
        public virtual DbSet<SatellitePosition> SatellitePositions { get; set; }
        public virtual DbSet<SatelliteRotation> SatelliteRotations { get; set; }
        public virtual DbSet<SatelliteShooting> SatelliteShootings { get; set; }
        public virtual DbSet<SatelliteToGroundStationTransfer> SatelliteToGroundStationTransfers { get; set; }
        public virtual DbSet<SatelliteToRetranslatorTransfer> SatelliteToRetranslatorTransfers { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=dbGlobe3DLight;Username=postgres;Password=user");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<GroundObject>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<GroundStation>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<InitialCondition>(entity =>
            {
                entity.Property(e => e.SunPositionXbegin).HasColumnName("SunPositionXBegin");

                entity.Property(e => e.SunPositionXend).HasColumnName("SunPositionXEnd");

                entity.Property(e => e.SunPositionYbegin).HasColumnName("SunPositionYBegin");

                entity.Property(e => e.SunPositionYend).HasColumnName("SunPositionYEnd");

                entity.Property(e => e.SunPositionZbegin).HasColumnName("SunPositionZBegin");

                entity.Property(e => e.SunPositionZend).HasColumnName("SunPositionZEnd");
            });

            modelBuilder.Entity<Retranslator>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<RetranslatorPosition>(entity =>
            {
                entity.HasIndex(e => e.RetranslatorId, "IX_RetranslatorPositions_RetranslatorId");

                entity.HasOne(d => d.Retranslator)
                    .WithMany(p => p.RetranslatorPositions)
                    .HasForeignKey(d => d.RetranslatorId);
            });

            modelBuilder.Entity<Satellite>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<SatellitePosition>(entity =>
            {
                entity.HasIndex(e => e.SatelliteId, "IX_SatellitePositions_SatelliteId");

                entity.HasOne(d => d.Satellite)
                    .WithMany(p => p.SatellitePositions)
                    .HasForeignKey(d => d.SatelliteId);
            });

            modelBuilder.Entity<SatelliteRotation>(entity =>
            {
                entity.HasIndex(e => e.SatelliteId, "IX_SatelliteRotations_SatelliteId");

                entity.HasOne(d => d.Satellite)
                    .WithMany(p => p.SatelliteRotations)
                    .HasForeignKey(d => d.SatelliteId);
            });

            modelBuilder.Entity<SatelliteShooting>(entity =>
            {
                entity.HasIndex(e => e.GroundObjectId, "IX_SatelliteShootings_GroundObjectId");

                entity.HasIndex(e => e.SatelliteId, "IX_SatelliteShootings_SatelliteId");

                entity.HasOne(d => d.GroundObject)
                    .WithMany(p => p.SatelliteShootings)
                    .HasForeignKey(d => d.GroundObjectId);

                entity.HasOne(d => d.Satellite)
                    .WithMany(p => p.SatelliteShootings)
                    .HasForeignKey(d => d.SatelliteId);
            });

            modelBuilder.Entity<SatelliteToGroundStationTransfer>(entity =>
            {
                entity.HasIndex(e => e.GroundStationId, "IX_SatelliteToGroundStationTransfers_GroundStationId");

                entity.HasIndex(e => e.SatelliteId, "IX_SatelliteToGroundStationTransfers_SatelliteId");

                entity.HasOne(d => d.GroundStation)
                    .WithMany(p => p.SatelliteToGroundStationTransfers)
                    .HasForeignKey(d => d.GroundStationId)
                    .HasConstraintName("FK_SatelliteToGroundStationTransfers_GroundStations_GroundStat~");

                entity.HasOne(d => d.Satellite)
                    .WithMany(p => p.SatelliteToGroundStationTransfers)
                    .HasForeignKey(d => d.SatelliteId);
            });

            modelBuilder.Entity<SatelliteToRetranslatorTransfer>(entity =>
            {
                entity.HasIndex(e => e.RetranslatorId, "IX_SatelliteToRetranslatorTransfers_RetranslatorId");

                entity.HasIndex(e => e.SatelliteId, "IX_SatelliteToRetranslatorTransfers_SatelliteId");

                entity.HasOne(d => d.Retranslator)
                    .WithMany(p => p.SatelliteToRetranslatorTransfers)
                    .HasForeignKey(d => d.RetranslatorId)
                    .HasConstraintName("FK_SatelliteToRetranslatorTransfers_Retranslators_Retranslator~");

                entity.HasOne(d => d.Satellite)
                    .WithMany(p => p.SatelliteToRetranslatorTransfers)
                    .HasForeignKey(d => d.SatelliteId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
