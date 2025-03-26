using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Common.Extensions
{
      public static class SqlDbExtension
      {


            public static IServiceCollection AddSqlDb(this IServiceCollection services, string conStr)
            {
                  return services.AddTransient<ISqlSugarClient>(p =>
                    {
                          SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
                          {
                                ConnectionString = conStr,
                                DbType = DbType.SqlServer,
                                IsAutoCloseConnection = true,
                                InitKeyType = InitKeyType.Attribute
                          });
                          return db;
                    });
            }
      }
}
