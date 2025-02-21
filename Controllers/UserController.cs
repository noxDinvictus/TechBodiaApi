using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBodiaApi.Attributes;
using TechBodiaApi.Services.Interfaces;

using DTO = TechBodiaApi.Data.Models.DTO.UserDTO;
using Payload = TechBodiaApi.Data.Models.Payload.UserPayload;

namespace TechBodiaApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/users")]
    [AllowAnonymous]
    public class UserController : BaseController
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userService)
        {
            _userService = userService;
        }

        [ApiValidationFilter]
        [HttpPost]
        public async Task<ActionResult<ResultOk<DTO>>> Create([FromBody] Payload payload)
        {
            try
            {
                var res = await _userService.Create(payload);
                return Success(res);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<ResultOk<string>>> GetAuthenticateToken(
            [FromBody] Payload payload
        )
        {
            try
            {
                var res = await _userService.GetAuthenticateToken(payload);
                return Success(res);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
