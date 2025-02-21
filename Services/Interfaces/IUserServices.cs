using DTO = TechBodiaApi.Data.Models.DTO.UserDTO;
using Payload = TechBodiaApi.Data.Models.Payload.UserPayload;

namespace TechBodiaApi.Services.Interfaces
{
    public interface IUserServices
    {
        Task<DTO> Create(Payload payload);

        Task<string> GetAuthenticateToken(Payload payload);
    }
}
