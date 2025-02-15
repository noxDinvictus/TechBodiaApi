using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBodiaApi.Attributes;
using TechBodiaApi.Services.Interfaces;
using DTO = TechBodiaApi.Data.Models.DTO.UserDTO;
using Payload = TechBodiaApi.Data.Models.Payload.UserPayload;

namespace TechBodiaApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/users")]
    public class UserController : BaseController
    {
        private readonly IUserServices userService;

        public UserController(IUserServices userService)
        {
            this.userService = userService;
        }

        [ApiValidationFilter]
        [HttpPost]
        public async Task<ActionResult<ResultOk<DTO>>> Create([FromBody] Payload dto)
        {
            try
            {
                var res = await userService.Create(dto);
                return Success(res);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<ResultOk<string>>> GetAuthenticateToken(
            [FromBody] Payload dto
        )
        {
            try
            {
                var res = await userService.GetAuthenticateToken(dto);
                return Success(res);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
