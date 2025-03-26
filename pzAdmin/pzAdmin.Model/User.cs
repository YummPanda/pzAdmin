using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Model
{
      [SugarTable("T_Users")]
      public class User
      {
            [SugarColumn(IsPrimaryKey = true)]
            public Guid Id { get; set; }
            public string Name { get; set; }
            [SugarColumn(IsNullable =true)]
            public string? NickName { get; set; }
            public string Password { get; set; }
            public DateTime CreateTime { get; set; }

            [SugarColumn(IsNullable = true)]
            public long RoleId { get; set; }

            [Navigate(NavigateType.OneToOne, nameof(RoleId))] // 一对一导航(实际是用户属于某个角色)
            public Role Role { get; set; }

      }
}
