using Microsoft.AspNetCore.Authorization;

namespace TechBodiaApi.Api
{
    public class AuthService
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }
    }
}
