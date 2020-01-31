using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gali.HL
{
    public class TokenHL
    {
        private string GetToken(string user, string nameIdentifier)
        {
            //var appSettings = new AppSettings();
            SecurityKey _issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(""));// appSettings.HMACKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user),
                    new Claim(ClaimTypes.NameIdentifier, nameIdentifier),
                    new Claim(ClaimTypes.Role,"User")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
