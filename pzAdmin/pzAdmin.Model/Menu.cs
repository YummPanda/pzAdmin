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

            public string Icon { get; set; }
            public string Path { get; set; }
            public string Discription { get; set; }
            [SugarColumn(IsNullable = true)]
            public int ParentId { get; set; }
            public List<Menu> Children { get; set; }
      }
}
