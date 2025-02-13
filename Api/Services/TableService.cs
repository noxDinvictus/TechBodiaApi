using TechBodiaApi.Services.Implementations;
using TechBodiaApi.Services.Interfaces;

namespace TechBodiaApi.Api
{
    public class TablesService
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Register services
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<INoteServices, NoteServices>();
        }
    }
}
