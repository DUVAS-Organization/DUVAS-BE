using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class UserDTO
    {
        [Key]
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Gmail { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Sex { get; set; }
        public string? ProfilePicture { get; set; }

        // Encrypted money fields
        public byte[]? EncryptedMoney { get; set; }
        public byte[]? MoneyIV { get; set; }

        // Money property that handles decryption
        public decimal Money
        {
            get
            {
                if (EncryptedMoney != null && MoneyIV != null)
                {
                    return DUVAS.EncryptionHelper.Decrypt(EncryptedMoney, MoneyIV);
                }
                return 0;
            }
        }

        public int? RoleAdmin { get; set; }
        public int? RoleUser { get; set; }
        public int? RoleLandlord { get; set; }
        public int? RoleService { get; set; }
    }

    public class EditProfileRequest
    {
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Sex { get; set; }
        public string? ProfilePicture { get; set; }
    }
}