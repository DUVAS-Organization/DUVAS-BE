using BusinessObject;
using System.ComponentModel.DataAnnotations;


namespace DUVAS
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string? UserName { get; set; }
        public string Name { get; set; }
        public string? Gmail { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Sex { get; set; }
        public string ProfilePicture { get; set; }
        public byte[]? EncryptedMoney { get; set; } // Thay Money
        public byte[]? MoneyIV { get; set; } // Lưu IV

        public int? RoleAdmin { get; set; }
        public int? RoleUser { get; set; }
        public int? RoleLandlord { get; set; }
        public int? RoleService { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; }
        public virtual ICollection<UserFeedback>? UserFeedbacks { get; set; }
        public virtual ICollection<ServiceLicense>? ServiceLicenses { get; set; }
        public virtual ICollection<LandlordLicense>? OwnerLicenses { get; set; }
        public virtual ICollection<Report>? Reports { get; set; }
        public virtual ICollection<RentalList>? RentalLists { get; set; }
        public virtual ICollection<WithdrawRequest> WithdrawRequests { get; set; }
        public virtual ICollection<Room>? Rooms { get; set; }
        public virtual ICollection<ServicePost>? ServicePosts { get; set; }
        public virtual ICollection<SavedPost>? SavedPosts { get; set; }
        public virtual ICollection<PriorityPackageServicePost>? PriorityPackageServicePosts { get; set; }
        public virtual ICollection<PriorityPackageRoom> PriorityPackageRooms { get; set; }
        public virtual ICollection<BankAccounts> BankAccounts { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        public User(string gmail, string userName, string name, string password, string address, string sex, string profilePicture, int? roleUser)
        {
            Gmail = gmail;
            UserName = userName;
            Name = name;
            Password = password;
            Address = address;
            Sex = sex;
            ProfilePicture = profilePicture;
            (byte[] encryptedMoney, byte[] iv) = EncryptionHelper.Encrypt(0); // Đã sửa kiểu dữ liệu
            EncryptedMoney = encryptedMoney;
            MoneyIV = iv;
            RoleUser = roleUser;
            RoleAdmin = 0;
            RoleLandlord = 0;
            RoleService = 0;
        }

        public User(string name, string? gmail, string profilePicture)
        {
            Name = name;
            Gmail = gmail;
            ProfilePicture = profilePicture;
            RoleUser = 1;
            RoleAdmin = 0;
            RoleLandlord = 0;
            RoleService = 0;
            (byte[] encryptedMoney, byte[] iv) = EncryptionHelper.Encrypt(0); // Đã sửa kiểu dữ liệu
            EncryptedMoney = encryptedMoney;
            MoneyIV = iv;
        }

        public string getRoleString()
        {
            if (RoleAdmin == 1)
            {
                return "Admin";
            }
            if (RoleLandlord == 1)
            {
                return "Landlord";
            }
            if (RoleService == 1)
            {
                return "Service";
            }
            return "User";
        }
    }
}