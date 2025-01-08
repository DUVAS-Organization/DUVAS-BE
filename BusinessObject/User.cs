using System.ComponentModel.DataAnnotations;
namespace DUVAS
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Name { get; set; }
        public string? Gmail { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Sex { get; set; }
        public string ProfilePicture { get; set; }
        public decimal Money { get; set; }

        public int? RoleAdmin { get; set; }
        public int? RoleUser { get; set; }
        public int? RoleLandlord { get; set; }
        public int? RoleService { get; set; }

        public virtual ICollection<Transaction>? Transactions { get; set; }
        public virtual ICollection<UserFeedback>? UserFeedbacks { get; set; }
        public virtual ICollection<ServiceLicense>? ServiceLicenses { get; set; }
        public virtual ICollection<OwnerLicense>? OwnerLicenses { get; set; }
        public virtual ICollection<Report>? Reports { get; set; }
        public virtual ICollection<RentalList>? RentalLists { get; set; }
        public User(string gmail, string userName, string name, string password, string address, string sex, string profilePicture, decimal money, int? roleUser)
        {
            Gmail = gmail;
            UserName = userName;
            Name = name;
            Password = password;
            Address = address;
            Sex = sex;
            ProfilePicture = profilePicture;
            Money = money;
            RoleUser = roleUser;
            RoleAdmin = 0;
            RoleLandlord = 0;
            RoleService = 0;
        }

        public User(string userName, string name, string? gmail, string profilePicture, decimal money)
        {
            UserName = userName;
            Name = name;
            Gmail = gmail;
            ProfilePicture = profilePicture;
            RoleUser = 1;
            RoleAdmin = 0;
            RoleLandlord = 0;
            RoleService = 0;
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
