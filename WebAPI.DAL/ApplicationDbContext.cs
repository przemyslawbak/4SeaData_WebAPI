using Microsoft.EntityFrameworkCore;
using System;
using Updater.Models;
using WebAPI.Models;

namespace WebAPI.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CompanyModel> Companies { get; set; }
        public DbSet<EmailModel> Emails { get; set; }
        public DbSet<VesselModel> Vessels { get; set; }
        public DbSet<AppSettings> Settings { get; set; }
        public DbSet<SeaModel> Seas { get; set; }
        public DbSet<PortModel> Ports { get; set; }
        public DbSet<UpdateLogModel> UpdatingLogs { get; set; }
        public DbSet<DailyStatisticsModel> Statistics { get; set; }
        public DbSet<VesselPort> VesselsPorts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //many-to-many

            modelBuilder.Entity<VesselPort>()
                .HasOne(vp => vp.VesselModel)
                .WithMany(vm => vm.VesselsPorts)
                .HasForeignKey(vu => vu.IMO);

            modelBuilder.Entity<VesselPort>()
                .HasOne(vp => vp.PortModel)
                .WithMany(vm => vm.VesselsPorts)
                .HasForeignKey(vu => vu.PortLocode);

            //one-to-many

            modelBuilder
                .Entity<DailyStatisticsModel>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CompanyModel>()
               .HasMany(c => c.EmailList)
               .WithOne(e => e.Company);

            //string[] array (EF Core 2.x)
            modelBuilder.Entity<VesselModel>()
             .Property(e => e.PreviousOwners)
             .HasConversion(
                 v => string.Join(",", v),
                  v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<VesselModel>()
            .Property(e => e.PreviousManagers)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<VesselModel>()
            .Property(e => e.DetailedType)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<CompanyModel>()
            .Property(e => e.FleetTypes)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<AppSettings>()
            .Property(e => e.VesselStatus)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<AppSettings>()
            .Property(e => e.VesselType)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<AppSettings>()
            .Property(e => e.VesselFlag)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<AppSettings>()
            .Property(e => e.VesselClass)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<AppSettings>()
            .Property(e => e.VesselBuilders)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<AppSettings>()
            .Property(e => e.VesselRegion)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<AppSettings>()
            .Property(e => e.VesselAisStatus)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<AppSettings>()
            .Property(e => e.VesselDetailedType)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<AppSettings>()
            .Property(e => e.CompanyCountry)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<AppSettings>()
            .Property(e => e.CompanyFleetTypes)
            .HasConversion(
                v => string.Join(",", v),
                 v => v.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
