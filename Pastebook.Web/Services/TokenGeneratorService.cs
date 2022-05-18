using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pastebook.Web.Services
{
    public interface ITokenGeneratorService
    {
        public string GenerateJwtToken(string username, string userAccountId);
    }

    public class TokenGeneratorService: ITokenGeneratorService
    {
        public string GenerateJwtToken(string username, string userAccountId)
        {
            var secret = "CFumQwG6PnUW6ODU8pWDYyWbGqFOsdPW";

            IDictionary<string, object> claims = new Dictionary<string, object>();
            claims.Add("name", username);

            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userAccountId) }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Claims = claims
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
