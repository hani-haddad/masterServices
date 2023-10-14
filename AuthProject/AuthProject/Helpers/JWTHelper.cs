using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AuthProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace AuthProject.Helpers
{
    public interface IJwtHelper
    {
        string GenerateToken(UserClaims user);
    }

    public class JWTHelper : IJwtHelper
    {
        private readonly AppSettings _appSettings;

        public JWTHelper(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GenerateToken(UserClaims user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id",user.Id),
                    new Claim("username", user.Username ),
                    new Claim("firstName",user.FirstName),
                    new Claim("lastName",user.LastName),
                    new Claim("email",user.Email),
                    new Claim("age",user.Age),
                    //new Claim("recivedInvitations", string.Join(",",user.RecivedInvitations.ToArray())),
                    //new Claim("adminOfGroups", string.Join(",",user.AdminOfGroups.ToArray())),
                    //new Claim("memberInGroups", string.Join(",",user.MemberInGroups.ToArray()) ),
                    new Claim("image",user.Image)
                    //new Claim("sentInvitations",string.Join(",",user.SentInvitations.ToArray()))
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
