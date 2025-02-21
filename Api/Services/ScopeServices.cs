using TechBodiaApi.Services.Implementations;
using TechBodiaApi.Services.Interfaces;

namespace TechBodiaApi.Api
{
    public class ScopeServices
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // db table services
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<INoteServices, NoteServices>();
            services.AddScoped<IActionsDataBaseServices, ActionsDataBaseServices>();
        }
    }
}
