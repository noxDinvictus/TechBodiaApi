using TechBodiaApi.Data.Definitions;
using TechBodiaApi.Data.Models.DTO;

namespace TechBodiaApi.Data.Models.Payload
{
    public class UserPayload
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Roles Role { get; set; } = Roles.User;

        public UserDTO ToDto()
        {
            return new UserDTO
            {
                Username = Username,
                Role = Role,
            };
        }
    }
}
