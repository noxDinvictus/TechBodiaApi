using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TechBodiaApi.Attributes
{
    /// <summary>
    /// Validates API request models and returns a structured BadRequest response if the model state is invalid.
    /// </summary>
    public sealed class ApiValidationFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Checks if the model state is valid before executing the action.
        /// </summary>
        /// <param name="context">Action executing context</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value.Errors.Select(err => err.ErrorMessage).ToArray()
                    );

                var response = new
                {
                    Message = "Validation failed",
                    Errors = errors
                };

                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}
