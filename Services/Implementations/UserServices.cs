using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TechBodiaApi.Data;
using TechBodiaApi.Services.Interfaces;
using DTO = TechBodiaApi.Data.Models.DTO.UserDTO;
using Model = TechBodiaApi.Data.Models.User;
using Payload = TechBodiaApi.Data.Models.Payload.UserPayload;

namespace TechBodiaApi.Services.Implementations
{
    public class UserServices : IUserServices
    {
        private readonly TechBodiaContext db;
        private readonly IConfiguration configuration;

        public UserServices(TechBodiaContext db, IConfiguration configuration)
        {
            this.db = db;
            this.configuration = configuration;
        }

        public async Task<DTO> Create(Payload dto)
        {
            using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                // Check if the username already exists
                bool usernameExists = await db.Users.AnyAsync(x => x.Username == dto.Username);
                if (usernameExists)
                {
                    throw new Exception("Username already exists");
                }

                var newItem = dto.ToDto().ToModel();

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(dto.Password, out passwordHash, out passwordSalt);

                newItem.PasswordHash = passwordHash;
                newItem.PasswordSalt = passwordSalt;

                db.Users.Add(newItem);
                await db.SaveChangesAsync();

                await transaction.CommitAsync();
                return newItem.ToDto();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"An error occurred while creating: {ex.Message}", ex);
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<string> GetAuthenticateToken(Payload dto)
        {
            if (string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password))
            {
                throw new ArgumentException("Username and password cannot be empty.");
            }

            var user = await db.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);

            if (user == null || !VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return GenerateJwtToken(user);
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        private string GenerateJwtToken(Model user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
