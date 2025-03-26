using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pzAdmin.Model;
using pzAdmin.Repository.Base;

namespace pzAdmin.Repository.MenuRep
{
      public interface IMenuRepository : IBaseRepository<Menu>
      {
            Task<List<Menu>> GetOwnMenuByRole(int roleId);
      }

}
