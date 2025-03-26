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
                  string[] parts = name.Split('.');
                  name = parts[0];
                  Type[] types = Assembly.LoadFrom(AppContext.BaseDirectory + name + ".Model.dll")
                        .GetTypes().Where(p => p.Namespace == name + ".Model").ToArray();

                  db.DbMaintenance.CreateDatabase();
                  db.CodeFirst.SetStringDefaultLength(255).InitTables(types);


                  //初始化一级菜单
                  var dashboard = new Menu()
                  {
                        Name = "控制台",
                        Icon = "Platform",
                        Path = "/dashboard",
                        Discription = "用于展示当前系统中的统计数据、统计报表及重要实时数据",
                        ParentId = null
                  };
                  var authMenu = new Menu
                  {
                        Name = "权限管理",
                        Icon = "Grid",
                        Path = null,
                        Discription = null,
                        ParentId = null
                  };
                  var vppzMenu = new Menu
                  {
                        Name = "DiDi陪诊",
                        Icon = "BellFilled",
                        Path = null,
                        Discription = null,
                        ParentId = null
                  };
                  db.Insertable(dashboard).ExecuteCommand();
                  db.Insertable(authMenu).ExecuteCommand();
                  db.Insertable(vppzMenu).ExecuteCommand();

                  //二级菜单
                  var adminMenu = new Menu
                  {
                        Name = "账号管理",
                        Icon = "Avatar",
                        Path = "/auth/admin",
                        Discription = "用于管理系统的管理员账号",
                        ParentId = 2
                  };

                  var groupMenu = new Menu
                  {
                        Name = "角色管理",
                        Icon = "Menu",
                        Path = "/auth/group",
                        Discription = "用于管理系统的角色权限",
                        ParentId = 2
                  };

                  // 陪护管理菜单（二级菜单）
                  var staffMenu = new Menu
                  {
                        Name = "陪护管理",
                        Icon = "Checked",
                        Path = "/vppz/staff",
                        Discription = "用于管理培护师信息",
                        ParentId = 3
                  };

                  // 订单管理菜单（二级菜单）
                  var orderMenu = new Menu
                  {
                        Name = "订单管理",
                        Icon = "List",
                        Path = "/vppz/order",
                        Discription = "用于管理陪护师的订单信息",
                        ParentId = 3
                  };
                  db.Insertable(adminMenu).ExecuteCommand();
                  db.Insertable(groupMenu).ExecuteCommand();
                  db.Insertable(staffMenu).ExecuteCommand();
                  db.Insertable(orderMenu).ExecuteCommand();

            }
      }
}
