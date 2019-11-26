using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gali.AppServer.Resources
{
    public class TokenRes
    {
        public Result<dynamic> TokenValid(string signedAndEncodedToken, string plainTextSecurityKey)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var signingKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(plainTextSecurityKey));
                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudiences = new string[] { "http://localhost:61101" },
                    ValidIssuers = new string[] { "http://localhost:61101" },
                    IssuerSigningKey = signingKey
                };

                SecurityToken validatedToken;
                var claims = tokenHandler.ValidateToken(signedAndEncodedToken,
                    tokenValidationParameters, out validatedToken);
                string user = claims.Claims.Where(item => item.Type.EndsWith("/nameidentifier")).FirstOrDefault().Value;
                string name = claims.Claims.Where(item => item.Type.EndsWith("/name")).FirstOrDefault().Value;
                string lastName = claims.Claims.Where(item => item.Type.EndsWith("/surname")).FirstOrDefault().Value;
                string email = claims.Claims.Where(item => item.Type.EndsWith("/emailaddress")).FirstOrDefault().Value;
                string city = "";
                if (claims.Claims.Where(item => item.Type.EndsWith("/stateorprovince")).Any())
                    city = claims.Claims.Where(item => item.Type.EndsWith("/stateorprovince")).FirstOrDefault().Value;

                var newTokenResult = this.GetToken(user, name, lastName, city, plainTextSecurityKey);
                return newTokenResult;

            }
            catch (Exception ex)
            {
                return new Result<dynamic> { State = ResultsStates.unsuccess, Message = "El Token no es valido" };
            }
        }

        public Result<dynamic> GetToken(string user, string name, string lastName, string city, string plainTextSecurityKey)
        {
            var signingKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(plainTextSecurityKey));
            var signingCredentials = new SigningCredentials(signingKey,
                SecurityAlgorithms.HmacSha256Signature);

            var x = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user),
                    new Claim(ClaimTypes.Name, name ),
                    new Claim(ClaimTypes.Surname, lastName),
                    new Claim(ClaimTypes.Email, user),
                    new Claim(ClaimTypes.StateOrProvince,city )
                };

            var claimsIdentity = new ClaimsIdentity(x, "Custom");

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = "http://localhost:61101",
                Issuer = "http://localhost:61101",
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddHours(12),
                SigningCredentials = signingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var plainToken = tokenHandler.CreateToken(securityTokenDescriptor);
            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);

            dynamic dataReturn = new
            {
                Token = signedAndEncodedToken,
                User = user,
                Name = name,
                LastName = lastName,
                Email = user,
                City = city
            };
            return new Result<dynamic>() { State = ResultsStates.success, Data = dataReturn };
        }

    }
}
