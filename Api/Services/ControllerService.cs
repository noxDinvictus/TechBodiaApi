namespace TechBodiaApi.Api.Services
{
    public class ControllerService
    {
        public void ConfigureServices(IServiceCollection services)
        {
            /**
             * Allows enum to be generated as string
             * Example
             *
             * [EnumMember(Value = "User")]
             * User = 1,
             *
             * When generated in the front end it will be
             *
             * User = 'User',
             *
             * NOTE: for objects you want to be ignored, use "using Newtonsoft.Json"; not "using System.Text.Json.Serialization;"
             */
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
                        .Json
                        .ReferenceLoopHandling
                        .Ignore;
                    options.SerializerSettings.Converters.Add(
                        new Newtonsoft.Json.Converters.StringEnumConverter()
                    );
                });

            // Add Swagger support for Newtonsoft.Json
            services.AddSwaggerGenNewtonsoftSupport(); // Explicitly add this for EnumMember to work
        }
    }
}
