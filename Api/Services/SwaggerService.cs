using Microsoft.OpenApi.Models;

namespace TechBodiaApi.Api.Extenstions
{
    public class SwaggerService
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            /**
             * Allows enum to be generated as string
             * Example
             *
             *  [EnumMember(Value = "User")]
             *  User = 1,
             *
             *  When generated in the front end it will be
             *
             *  User = 'User',
             *
            */
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "TechBodia API", Version = "v1" });
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Enter 'Bearer {token}'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                    }
                );

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer",
                                },
                            },
                            new List<string>()
                        },
                    }
                );

                // Add this line to respect EnumMember values
                options.UseAllOfToExtendReferenceSchemas();
            });
            // Add Swagger support for Newtonsoft.Json
            services.AddSwaggerGenNewtonsoftSupport(); // Explicitly add this for EnumMember to work
        }
    }
}
