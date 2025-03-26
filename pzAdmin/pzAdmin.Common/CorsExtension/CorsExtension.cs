using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Common.CorsExtension
{
      public static class CorsExtension
      {
            public static void AddCorsExtension(this IServiceCollection services, string[] urls)
            {
                  services.AddCors(options =>
                  {
                        options.AddPolicy("MyCorsPolicy", builder =>
                        {
                              builder.WithOrigins(urls)
                                    .AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader();
                        });
                  });
            }
            public static void UseCorsExtension(this IApplicationBuilder app)
            {
                  app.UseCors("MyCorsPolicy");
            }
      }
}
