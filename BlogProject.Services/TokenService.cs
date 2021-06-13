using BlogProject.Models.Account;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        public TokenService(IConfiguration config) 
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            _issuer = config["Jwt:Issuer"];
        }

        public string CreateToken(ApplicationUserIdentity user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.ApplicationUserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // before creating the token the params are created
            var token = new JwtSecurityToken(
                _issuer,
                _issuer,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
