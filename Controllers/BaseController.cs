using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace TechBodiaApi.Controllers
{
    public class BaseController : ControllerBase
    {

        public class ResultOk<T>
        {
            public T Result { get; set; }
            public int StatusCode { get; set; }
        }
        protected ActionResult Success()
        {
            return Ok();
        }

        protected ActionResult<ResultOk<T>> Success<T>(T result)
        {
            return new ResultOk<T>
            {
                Result = result,
                StatusCode = 200
            };
        }

        protected ActionResult HandleError(Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }

        protected Guid GetCurrentUserId()
        {
            var userIdClaim = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out Guid userId))
            {
                return userId;
            }

            throw new InvalidOperationException("Invalid or missing User ID in token.");
        }
    }
}
