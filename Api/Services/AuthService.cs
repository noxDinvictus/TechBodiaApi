using Microsoft.AspNetCore.Authorization;
using TechBodiaApi.Data.Definitions;

namespace TechBodiaApi.Api
{
    public class AuthService
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Require authentication for all endpoints unless explicitly allowed
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                // Policy for User role validation
                options.AddPolicy(nameof(Roles.User), policy =>
                    policy.RequireAuthenticatedUser()
                          .RequireRole(Roles.User.ToString()));
            });
        }
    }
}
