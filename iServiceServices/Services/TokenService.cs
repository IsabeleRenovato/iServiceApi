using iServiceRepositories.Repositories.Models;
using iServiceServices.Services.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace iServiceServices.Services
{
    public static class TokenService
    {
        public const string UserId = "UserId";
        public const string UserProfileId = "UserProfileId";
        public static string GenerateToken((User User, UserRole UserRole, UserProfile UserProfile) user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.User.Email),
                    new Claim(ClaimTypes.Role, user.UserRole.Name),
                    new Claim(UserId, user.User.UserId.ToString()),
                    new Claim(UserProfileId, user.UserProfile.UserProfileId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
