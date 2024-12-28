using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using DUVAS;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace DUVAS
{
    public class ApplicationDbContext : DbContext
    {
        

        // DbSets
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomLicense> RoomLicenses { get; set; }
        public virtual DbSet<RentalList> RentalLists { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<ServicePost> ServicePosts { get; set; }
        public virtual DbSet<UserFeedback> UserFeedbacks { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ServiceLicense> ServiceLicenses { get; set; }
        public virtual DbSet<CategoryRoom> CategoryRooms { get; set; }
        public virtual DbSet<ServiceFeedback> ServiceFeedbacks { get; set; }
        public virtual DbSet<CategoryService> CategoryServices { get; set; }
        public virtual DbSet<OwnerLicense> OwnerLicenses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }
        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DBString"));
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API configurations


            // Room - Building
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Building)
                .WithMany(b => b.Rooms)
                .HasForeignKey(r => r.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            // RoomLicense - Room
            modelBuilder.Entity<RoomLicense>()
                .HasOne(rl => rl.Room)
                .WithMany(r => r.RoomLicenses)
                .HasForeignKey(rl => rl.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // RentalList - Room
            modelBuilder.Entity<RentalList>()
                .HasOne(rl => rl.Room)
                .WithMany(r => r.RentalLists)
                .HasForeignKey(rl => rl.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // RentalList - User (as IDThue)
            modelBuilder.Entity<RentalList>()
                .HasOne(rl => rl.User)
                .WithMany(u => u.RentalLists)  // Chỉnh sửa: phải là RentalLists thay vì Transactions
                .HasForeignKey(rl => rl.RenterID)
                .OnDelete(DeleteBehavior.Restrict);

            // RentalList - Contract
            modelBuilder.Entity<RentalList>()
                .HasOne(rl => rl.Contract)
                .WithMany(c => c.RentalLists)
                .HasForeignKey(rl => rl.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            // Transaction - Room
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Room)
                .WithMany()
                .HasForeignKey(t => t.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction - ServicePost
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ServicePost)
                .WithMany()
                .HasForeignKey(t => t.ServicePostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction - User (as IDThue)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.IDThue)
                .OnDelete(DeleteBehavior.Restrict);

            // UserFeedback - User
            modelBuilder.Entity<UserFeedback>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.UserFeedbacks)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ServiceLicense - User
            modelBuilder.Entity<ServiceLicense>()
                .HasOne(sl => sl.User)
                .WithMany(u => u.ServiceLicenses)
                .HasForeignKey(sl => sl.UserId);

            // ServicePost - CategoryService
            modelBuilder.Entity<ServicePost>()
                .HasOne(sp => sp.CategoryService)
                .WithMany(cs => cs.ServicePosts)
                .HasForeignKey(sp => sp.CategoryServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // ServiceFeedback - ServicePost
            modelBuilder.Entity<ServiceFeedback>()
                .HasOne(sf => sf.ServicePost)
                .WithMany(sp => sp.ServiceFeedbacks)
                .HasForeignKey(sf => sf.ServicePostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Report - User
            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Report - Room
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Room)
                .WithMany()
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // Report - ServicePost
            modelBuilder.Entity<Report>()
                .HasOne(r => r.ServicePost)
                .WithMany()
                .HasForeignKey(r => r.ServicePostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Report - Transaction
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Transaction)
                .WithMany()
                .HasForeignKey(r => r.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            // OwnerLicense - User
            modelBuilder.Entity<OwnerLicense>()
                .HasOne(ol => ol.User)
                .WithMany(u => u.OwnerLicenses)
                .HasForeignKey(ol => ol.UserId);
        }

    }
}
