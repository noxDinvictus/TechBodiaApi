using System.ComponentModel.DataAnnotations;
using TechBodiaApi.Data.Models.DTO;

namespace TechBodiaApi.Data.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = new byte[64];
        public byte[] PasswordSalt { get; set; } = new byte[128];
        public DateTime CreatedAt { get; set; }

        public UserDTO ToDto()
        {
            return new UserDTO
            {
                UserId = UserId,
                Username = Username,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
