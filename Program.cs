using System.Text;
using Delta;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TechBodiaApi.Api;
using TechBodiaApi.Api.Extenstions;
using TechBodiaApi.Api.Services;
using TechBodiaApi.Data;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");

const string corsPolicyName = "CORS, you are either Screaming OR Crying :D";

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Database connection string is missing.");
}

builder.Services.AddDbContext<TechBodiaContext>(options => options.UseSqlServer(connectionString));

// JWT Authentication Configuration
var jwtSettings = configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new Exception("JWT key is missing from configuration.");
}

var key = Encoding.UTF8.GetBytes(jwtKey);

// enable JWT validation
var tokenAuthService = new TokenAuthService();
tokenAuthService.ConfigureServices(builder.Services, key);

// Enable Authorization
var authService = new AuthService();
authService.ConfigureServices(builder.Services);

// Register Services, db tables
// NOTE: newly created table should be added here
var collectionService = new ScopeServices();
collectionService.ConfigureServices(builder.Services);

// Enable CORS
var corsPolicyService = new CorsPolicyService();
corsPolicyService.ConfigureServices(builder.Services, corsPolicyName);

// Add Controllers
builder.Services.AddControllers();

var versioning = new Versioning();
versioning.ConfigureServices(builder.Services);

// Configure Swagger with JWT support
var swaggerService = new SwaggerService();
swaggerService.ConfigureServices(builder.Services);

// SOURCE: https://github.com/SimonCropp/Delta?tab=readme-ov-file
// NOTE: this makes fetching faster
builder.Services.AddScoped(_ => new SqlConnection(connectionString));

var app = builder.Build();

// SOURCE: https://www.youtube.com/watch?v=sLoTReccvPw
// NOTE: this makes fetching faster
app.UseDelta();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();

// Apply CORS before authentication & authorization
app.UseCors(corsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
