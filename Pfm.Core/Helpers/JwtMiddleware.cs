using System;
using Pfm.Core.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Pfm.Core.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace Pfm.Core.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IPresence presence)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachUserToContext(context, presence, token);

            await _next(context);
        }

        private static void AttachUserToContext(HttpContext context, IPresence presence, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(AppSetting.KeySecret!);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var refreshToken = jwtToken.Claims.First(x => x.Type == "refresh_token").Value;
                context.Items["User"] = null;
                if (refreshToken == "false")
                {
                    var userId = jwtToken.Claims.First(x => x.Type == "id").Value;
                    context.Items["User"] = presence.GetUser(int.Parse(userId));
                }
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
        public static string GenerateJwtToken(TbUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSetting.KeySecret!);
            var expires = DateTime.UtcNow.AddMonths(1);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", user.IdUser.ToString()),
                    new Claim("email", user.Email),
                    new Claim("refresh_token", "false"),
                   }
                ),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        
    }
}