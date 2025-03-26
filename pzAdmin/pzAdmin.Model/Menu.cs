using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Model
{
      [SugarTable("T_Menu")]
      public class Menu
      {
            [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
            public int Id { get; set; }

            public string Name { get; set; }

            public string? Icon { get; set; }
            [SugarColumn(IsNullable = true)]
            public string? Path { get; set; }
            [SugarColumn(IsNullable = true)]
            public string? Discription { get; set; }
            [SugarColumn(IsNullable = true)]
            public int? ParentId { get; set; }
            [SugarColumn(IsNullable = true)]
            public List<Menu> Children { get; set; }

            // 导航属性（反向多对多）
            [Navigate(typeof(RoleMenu), nameof(RoleMenu.MenuId), nameof(RoleMenu.RoleId))]
            public List<Role> Roles { get; set; }
      }
}
