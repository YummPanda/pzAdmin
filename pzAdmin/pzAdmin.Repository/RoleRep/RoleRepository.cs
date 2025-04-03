using pzAdmin.Model;
using pzAdmin.Repository.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Repository.RoleRep
{
      public class RoleRepository : BaseRepository<Role>, IRoleRepository
      {
            public RoleRepository(ISqlSugarClient db) : base(db)
            {
            }
      }
}
