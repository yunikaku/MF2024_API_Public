using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using MF2024_API.Interfaces;
using MF2024_API.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;




namespace MF2024_API.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly Mf2024apiDbContext _context;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            _context = new Mf2024apiDbContext();
        }



        public string CreateToken(User user,IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,string.Join(",",roles))


            };


            

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
 //               Claims = roles.Select(r => new KeyValuePair<string, object>(ClaimTypes.Role, r)).ToDictionary(),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            

            return tokenHandler.WriteToken(token);
        }

        public string CreateTokenReservation( string code)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,code)
                
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //リフレッシュトークン発行用
        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        //リフレッシュトークン保存用（DB）
        public void SaveRefreshToken(string token, string userId)
        {
            try
            {

                var user = _context.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    user.RefreshToken = token;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        //リフレッシュトークン検証用
        public bool VerifyRefreshToken(string token,User user)
        {
            if (user.RefreshToken == token)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //アクセストークン検証用
        public ClaimsPrincipal VerifyToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JWT:SigningKey"]);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _config["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["JWT:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                return principal;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}