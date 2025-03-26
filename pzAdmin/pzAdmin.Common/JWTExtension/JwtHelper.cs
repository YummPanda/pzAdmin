using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Common.JWTExtension
{
      public static class JwtHelper
      {
            public static string BuildJwtToken(IEnumerable<Claim> claims, JwtOptions options)
            {
                  TimeSpan ExpiryTime = TimeSpan.FromSeconds(options.ExpireSeconds);
                  var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
                  var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                  var tokenDescriptor = new JwtSecurityToken(options.Issuer, options.Audience, claims, expires: DateTime.Now.Add(ExpiryTime), signingCredentials: credentials);
                  return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            }
      }
}
