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

            /// <summary>
            /// 获取用户菜单列表
            /// </summary>
            /// <param name="userId"></param>
            /// <returns></returns>
            public async Task<List<OwnMenuRes>> GetOwnMenuListAsync(int roleId)
            {
                  var allMenuList = await menuRepository.getAll();
                  var firstMenu = allMenuList.Where(m => m.ParentId == null).ToList();
                  var ownMenuList = await menuRepository.GetOwnMenuByRole(roleId);
                  var ownMenuListIds = ownMenuList.Select(m => m.Id).ToList();
                  var ownMenuResList = new List<OwnMenuRes>();
                  foreach (var menu in firstMenu)
                  {
                        var ownMenu = new OwnMenuRes
                        { 
                              Id = menu.Id,
                              Label = menu.Name,
                        };
                        if (!ownMenuListIds.Contains(menu.Id))
                        {
                              ownMenu.Disabled= true;
                        }
                        BuildMenuTree(menu, ownMenu, allMenuList, ownMenuListIds);
                        ownMenuResList.Add(ownMenu);
                  }
                  return ownMenuResList;
            }
            private void BuildMenuTree(Menu menu, OwnMenuRes ownMenu, List<Menu> allMenuList, List<int> ownMenuListIds)
            {
                  var childMenuList = allMenuList.Where(m => m.ParentId == menu.Id).ToList();
                  foreach (var childMenu in childMenuList)
                  {
                        var childOwnMenu = new OwnMenuRes
                        {
                              Id = childMenu.Id,
                              Label = childMenu.Name,
                        };
                        if (!ownMenuListIds.Contains(childMenu.Id))
                        {
                              childOwnMenu.Disabled = true;
                        }
                        ownMenu.Children.Add(childOwnMenu);
                        BuildMenuTree(childMenu, childOwnMenu, allMenuList, ownMenuListIds);
                  }

            }
      }
}
