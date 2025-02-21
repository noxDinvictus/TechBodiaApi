namespace TechBodiaApi.Api.Extenstions
{
    public class CorsPolicyService
    {
        public void ConfigureServices(IServiceCollection services, string corsPolicyName)
        {
            var allowedOrigins = new List<string>
            {
                "http://localhost:3000",
                "https://noxdinvictus.github.io",
            };

            services.AddCors(options =>
            {
                options.AddPolicy(
                    corsPolicyName,
                    policy =>
                    {
                        policy
                            .WithOrigins(allowedOrigins.ToArray())
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .Build();
                    }
                );
            });
        }
    }
}
