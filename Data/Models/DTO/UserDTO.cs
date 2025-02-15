using TechBodiaApi.Data.Definitions;

namespace TechBodiaApi.Data.Models.DTO
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Roles Role { get; set; }

        public User ToModel()
        {
            return new User
            {
                UserId = UserId,
                Username = Username,
                Role = Role,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
