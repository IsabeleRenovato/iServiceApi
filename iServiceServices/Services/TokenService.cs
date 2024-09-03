using iServiceRepositories.Repositories.Models;
using iServiceServices.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace iServiceServices.Services
{
    public class TokenInfo
    {
        public string UserId { get; set; }
        public string UserProfileId { get; set; }
        public string AddressId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
    public static class TokenService
    {
        public const string UserId = "UserId";
        public const string UserProfileId = "UserProfileId";
        public const string AddressId = "AddressId";

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
                new Claim(AddressId, user.UserProfile.AddressId?.ToString() ?? "")
            }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static string GetJwtToken(HttpContext httpContext)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                return authorizationHeader.Substring("Bearer ".Length).Trim();
            }

            return string.Empty;
        }

        public static TokenInfo GetTokenInfo(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(token))
            {
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var claims = jwtToken.Claims.ToList();

                var tokenInfo = new TokenInfo
                {
                    UserId = claims.FirstOrDefault(c => c.Type == UserId)?.Value,
                    UserProfileId = claims.FirstOrDefault(c => c.Type == UserProfileId)?.Value,
                    AddressId = claims.FirstOrDefault(c => c.Type == AddressId)?.Value,
                    Name = claims.FirstOrDefault(c => c.Type == "unique_name")?.Value,
                    Role = claims.FirstOrDefault(c => c.Type == "role")?.Value
                };

                return tokenInfo;
            }
            return null;
        }
    }
}
