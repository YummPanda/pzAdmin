using pzAdmin.Model;
using pzAdmin.Repository.Base;
using pzAdmin.Service.Base;

namespace pzAdmin.Service.RoleServ
{
      public class RoleService : BaseService<Role>, IRoleService
      {
            public RoleService(IBaseRepository<Role> baseRepository) : base(baseRepository)
            {
            }
      }
}
