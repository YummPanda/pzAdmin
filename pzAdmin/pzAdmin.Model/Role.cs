using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Model
{
      [SugarTable("T_Roles")]
      public class Role
      {
            [SugarColumn(IsPrimaryKey = true,IsIdentity = true)]
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime CreatTime { get; set; }

            [Navigate(NavigateType.OneToMany, nameof(User.RoleId))] // 一对多导航
            public List<User> Users { get; set; }

            [Navigate(typeof(RoleMenu), nameof(RoleMenu.RoleId), nameof(RoleMenu.MenuId))] // 多对多导航
            public List<Menu> Menus { get; set; }
      }
}
