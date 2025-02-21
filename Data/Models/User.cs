using System.ComponentModel.DataAnnotations;
using TechBodiaApi.Data.Definitions;
using TechBodiaApi.Data.Models.DTO;

namespace TechBodiaApi.Data.Models
{
    public class User : BaseClass
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = new byte[64];
        public byte[] PasswordSalt { get; set; } = new byte[128];
        public Roles Role { get; set; } = Roles.User;

        public UserDTO ToDTO()
        {
            return new UserDTO
            {
                UserId = UserId,
                Username = Username,
                Role = Role,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
