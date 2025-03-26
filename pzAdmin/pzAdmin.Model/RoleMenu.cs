using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Model
{
      [SugarTable("T_RoleMenus")]
      public class RoleMenu
      {
            [SugarColumn(IsPrimaryKey = true)]
            public int RoleId { get; set; }

            [SugarColumn(IsPrimaryKey = true)]
            public int MenuId { get; set; }
      }
}
