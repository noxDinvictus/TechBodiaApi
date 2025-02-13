namespace TechBodiaApi.Api.Extenstions
{
    public class CorsPolicyService
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowFrontend",
                    policy =>
                    {
                        policy
                            .WithOrigins("https://localhost:3000", "https://noxdinvictus.github.io")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    }
                );
            });
        }
    }
}
