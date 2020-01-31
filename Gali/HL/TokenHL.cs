using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Gali.HL
{
    public class TokenHL
    {
        private string GetToken(string user, string nameIdentifier, string roles, int lifeTime, string plainTextSecurityKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityKey _issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(plainTextSecurityKey));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user),
                    new Claim(ClaimTypes.NameIdentifier, nameIdentifier),
                    new Claim(ClaimTypes.Role,roles)
                }),
                Expires = DateTime.UtcNow.AddMinutes(lifeTime),
                SigningCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public Result<string> TokenValid(string signedAndEncodedToken, int lifeTime, string plainTextSecurityKey)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var signingKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(plainTextSecurityKey));
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = signingKey
                };

                SecurityToken validatedToken;
                var claims = tokenHandler.ValidateToken(signedAndEncodedToken,
                    tokenValidationParameters, out validatedToken);
                string user = claims.Claims.Where(item => item.Type.EndsWith("/name")).FirstOrDefault().Value;
                string nameIdentifier = claims.Claims.Where(item => item.Type.EndsWith("/nameidentifier")).FirstOrDefault().Value;
                string role = claims.Claims.Where(item => item.Type.EndsWith("/role")).FirstOrDefault().Value;
                //string name = claims.Claims.Where(item => item.Type.EndsWith("/name")).FirstOrDefault().Value;
                //string lastName = claims.Claims.Where(item => item.Type.EndsWith("/surname")).FirstOrDefault().Value;
                //string email = claims.Claims.Where(item => item.Type.EndsWith("/emailaddress")).FirstOrDefault().Value;
                //string city = "";
                //if (claims.Claims.Where(item => item.Type.EndsWith("/stateorprovince")).Any())
                //    city = claims.Claims.Where(item => item.Type.EndsWith("/stateorprovince")).FirstOrDefault().Value;

                var newTokenResult = this.GetToken(user, nameIdentifier, role, lifeTime, plainTextSecurityKey);
                return new Result<string> { State = ResultsStates.success, Message = "El Token fue creado con exito." };
            }
            catch (Exception ex)
            {
                return new Result<string> { State = ResultsStates.unsuccess, Message = "El Token no es valido." };
            }
        }
    }
}
