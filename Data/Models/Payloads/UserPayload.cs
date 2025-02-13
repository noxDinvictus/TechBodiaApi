using TechBodiaApi.Data.Models.DTO;

namespace TechBodiaApi.Data.Models.Payload
{
    public class UserPayload
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public UserDTO ToDto()
        {
            return new UserDTO
            {
                Username = Username,
            };
        }
    }
}
