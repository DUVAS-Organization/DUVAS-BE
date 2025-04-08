using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using DUVAS;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using BusinessObject;

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
        public virtual DbSet<RentalServiceList> RentalServiceLists { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<WithdrawRequest> WithdrawRequests { get; set; }
        public virtual DbSet<ServicePost> ServicePosts { get; set; }
        public virtual DbSet<UserFeedback> UserFeedbacks { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ServiceLicense> ServiceLicenses { get; set; }
        public virtual DbSet<CategoryRoom> CategoryRooms { get; set; }
        public virtual DbSet<ServiceFeedback> ServiceFeedbacks { get; set; }
        public virtual DbSet<CategoryService> CategoryServices { get; set; }
        public virtual DbSet<LandlordLicense> LandlordLicenses { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<CategoryPriorityPackageRoom> CategoryPriorityPackageRooms { get; set; }
        public virtual DbSet<CategoryPriorityPackageServicePost> CategoryPriorityPackageServicePosts { get; set; }
        public virtual DbSet<PriorityPackageRoom> PriorityPackageRooms { get; set; }
        public virtual DbSet<PriorityPackageServicePost> PriorityPackageServicePosts { get; set; }
        public virtual DbSet<InsiderTrading> InsiderTradings { get; set; }
        public DbSet<SavedPost> SavedPosts { get; set; }
        public virtual DbSet<BankAccounts> BankAccounts { get; set; }
        public virtual DbSet<AuthorizationContract> AuthorizationContracts { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }

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

            // Room - User
            modelBuilder.Entity<Room>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rooms)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Ngăn vòng lặp khi xóa User

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

            modelBuilder.Entity<ServicePost>()
                .HasOne(r => r.User)
                .WithMany(u => u.ServicePosts)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Transaction-User
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User) // One Transaction has one User
                .WithMany(u => u.Transactions) // One User has many Transactions
                .HasForeignKey(t => t.UserId) // Foreign key in Transaction
                .OnDelete(DeleteBehavior.Cascade); // Optional: Configure delete behavior

            // WithDrawRequest - User
            modelBuilder.Entity<WithdrawRequest>()
                .HasOne(w => w.User) // WithdrawRequest has one User
                .WithMany(u => u.WithdrawRequests) // User has many WithdrawRequests
                .HasForeignKey(w => w.UserId) // Foreign key in WithdrawRequest
                .OnDelete(DeleteBehavior.Cascade); // Configure delete behavior
            // WithDrawRequest - Transaction
            modelBuilder.Entity<WithdrawRequest>()
                .HasOne(w => w.Transaction)
                .WithOne()  // One withdraw request has one transaction
                .HasForeignKey<WithdrawRequest>(w => w.TransactionId)  // TransactionId in WithdrawRequest is the foreign key
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ giữa Message và User (UserSend)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.UserSend)
                .WithMany() // Nếu không có navigation property ở User
                .HasForeignKey(m => m.UserSendID)
                .OnDelete(DeleteBehavior.NoAction); // Sử dụng NoAction để tắt cascade delete

            // Cấu hình quan hệ giữa Message và User (UserGet)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.UserGet)
                .WithMany() // Nếu không có navigation property ở User
                .HasForeignKey(m => m.UserGetID)
                .OnDelete(DeleteBehavior.NoAction); // Sử dụng NoAction

            //Conversion string for enum type
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Status)
                .HasConversion<string>();
            modelBuilder.Entity<WithdrawRequest>()
                .Property(t => t.Status)
                .HasConversion<string>();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PriorityPackageRoom>()
               .HasOne(p => p.User)
               .WithMany(u => u.PriorityPackageRooms) // Đảm bảo User có danh sách PriorityPackageRooms
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Restrict); // Tránh vòng lặp

            modelBuilder.Entity<PriorityPackageRoom>()
                .HasOne(p => p.Room)
                .WithMany(r => r.PriorityPackageRooms) // Đảm bảo Room có danh sách PriorityPackageRooms
                .HasForeignKey(p => p.RoomId) // Chỉ sử dụng RoomId
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PriorityPackageRoom>()
                .HasOne(p => p.CategoryPriorityPackageRoom)
                .WithMany(c => c.PriorityPackageRooms)
                .HasForeignKey(p => p.CategoryPriorityPackageRoomId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PriorityPackageServicePost>()
                .HasOne(p => p.User)
                .WithMany(u => u.PriorityPackageServicePosts) // Đảm bảo User có danh sách PriorityPackageServicePosts
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PriorityPackageServicePost>()
                .HasOne(p => p.ServicePost)
                .WithMany(s => s.PriorityPackageServicePosts) // Đảm bảo ServicePost có danh sách PriorityPackageServicePosts
                .HasForeignKey(p => p.ServicePostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PriorityPackageServicePost>()
                .HasOne(p => p.CategoryPriorityPackageServicePost)
                .WithMany(c => c.PriorityPackageServicePosts)
                .HasForeignKey(p => p.CategoryPriorityPackageServicePostId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SavedPost>()
                .HasOne(sp => sp.Room)
                .WithMany(r => r.SavedPosts)
                .HasForeignKey(sp => sp.RoomId)
                .OnDelete(DeleteBehavior.Restrict); // Không xóa nếu còn SavedPost liên quan

            modelBuilder.Entity<SavedPost>()
                .HasOne(sp => sp.User)
                .WithMany(u => u.SavedPosts)
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa User thì xóa luôn SavedPost liên quan

            modelBuilder.Entity<SavedPost>()
               .HasOne(sp => sp.ServicePost)
               .WithMany(s => s.SavedPosts)
               .HasForeignKey(sp => sp.ServicePostId)
               .OnDelete(DeleteBehavior.Restrict); // Xóa ServicePost thì xóa luôn SavedPost

            modelBuilder.Entity<AuthorizationContract>()
               .HasOne(a => a.PartyA)
               .WithMany()
               .HasForeignKey(a => a.PartyAId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AuthorizationContract>()
                .HasOne(a => a.PartyB)
                .WithMany()
                .HasForeignKey(a => a.PartyBId)
                .OnDelete(DeleteBehavior.NoAction); // Đổi từ Cascade thành NoAction

            modelBuilder.Entity<Notification>()
               .HasOne(n => n.User)
               .WithMany(u => u.Notifications)
               .HasForeignKey(n => n.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .Property(n => n.Type)
                .HasConversion<string>();
        }

    }
}