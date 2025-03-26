using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pzAdmin.Model;
using SqlSugar;
using System.Reflection;

namespace pzAdmin.WebAPI.Controllers
{
      [Route("api/[controller]")]
      [ApiController]
      public class InitDataBaseController : ControllerBase
      {
            private readonly ISqlSugarClient db;

            public InitDataBaseController(ISqlSugarClient db)
            {
                  this.db = db;
            }

            [HttpPost]
            public void InitDataBase()
            {

                  string name = Assembly.GetExecutingAssembly().GetName().Name;
                  string[] parts=name.Split('.');
                  name = parts[0];
                  Type[] types = Assembly.LoadFrom(AppContext.BaseDirectory +name + ".Model.dll")
                        .GetTypes().Where(p => p.Namespace == name + ".Model").ToArray();
                
                  db.DbMaintenance.CreateDatabase();
                  db.CodeFirst.SetStringDefaultLength(255).InitTables(types);



                  new Menu()
                  {
                        Name:"控制台",

                  }
            }
      }
}
