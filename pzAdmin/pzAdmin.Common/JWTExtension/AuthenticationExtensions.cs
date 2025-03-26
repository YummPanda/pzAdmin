using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace pzAdmin.Common.JWTExtension
{
      public static class AuthenticationExtensions
      {
            /// <summary>
            /// 给Service一个扩展方法，注册配置JWT
            /// </summary>
            /// <param name="services"></param>
            /// <param name="jwtOptions">配置项的参数</param>
            /// <returns></returns>
            public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
            {
                  return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(j =>
                        {
                              j.TokenValidationParameters = new()
                              {
                                    ValidateIssuer = true,        //是否验证发行人
                                    ValidateAudience = true, //是否验证被发行人
                                    ValidateLifetime = true, //是否验证生命周期
                                    ValidateIssuerSigningKey = true,//是否验证签名秘钥
                                    ValidIssuer = jwtOptions.Issuer,
                                    ValidAudience = jwtOptions.Audience,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                              };
                        });
            }
      }
}
