using Microsoft.Extensions.Options;
using pzAdmin.Common;
using pzAdmin.Common.JWTExtension;
using pzAdmin.Common.req;
using pzAdmin.Common.Res;
using pzAdmin.Model;
using pzAdmin.Repository.Base;
using pzAdmin.Repository.MenuRep;
using pzAdmin.Repository.UserRep;
using pzAdmin.Service.Base;
using System.Security.Claims;

namespace pzAdmin.Service.MenuServ
{
      public class MenuService : BaseService<Menu>, IMenuService
      {
            private readonly IMenuRepository menuRepository;

            public MenuService(IBaseRepository<Menu> baseRepository, IMenuRepository menuRepository) : base(baseRepository)
            {
                  this.menuRepository = menuRepository;
            }

      }
}
