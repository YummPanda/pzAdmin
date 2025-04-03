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
            /// 获取用户菜单列表 并标注用户是否拥有该菜单
            /// </summary>
            /// <param name="userId">角色ID</param>
            /// <returns></returns>
            public async Task<List<OwnMenuRes>> GetOwnMenuListAsync(int roleId)
            {
                  // 获取所有菜单列表
                  var allMenuList = await menuRepository.getAll();
                  // 获取一级菜单列表
                  var firstMenu = allMenuList.Where(m => m.ParentId == null).ToList();
                  // 获取指定角色拥有的菜单列表
                  var ownMenuList = await menuRepository.GetOwnMenuByRole(roleId);
                  // 获取指定角色拥有的菜单ID列表
                  var ownMenuListIds = ownMenuList.Select(m => m.Id).ToList();
                  // 初始化结果菜单列表
                  var ownMenuResList = new List<OwnMenuRes>();
                  // 遍历一级菜单，构建菜单树
                  foreach (var menu in firstMenu)
                  {
                        var ownMenu = new OwnMenuRes
                        {
                              Id = menu.Id,
                              Label = menu.Name,
                        };
                        // 如果菜单不在指定角色拥有的菜单ID列表中，则禁用该菜单
                        if (!ownMenuListIds.Contains(menu.Id))
                        {
                              ownMenu.Disabled = true;
                        }
                        // 递归构建子菜单树
                        BuildMenuTree(menu, ownMenu, allMenuList, ownMenuListIds);
                        ownMenuResList.Add(ownMenu);
                  }
                  return ownMenuResList;
            }

            private void BuildMenuTree(Menu menu, OwnMenuRes ownMenu, List<Menu> allMenuList, List<int> ownMenuListIds)
            {
                  // 获取当前菜单的子菜单列表
                  var childMenuList = allMenuList.Where(m => m.ParentId == menu.Id).ToList();
                  // 遍历子菜单列表，递归构建菜单树
                  foreach (var childMenu in childMenuList)
                  {
                        var childOwnMenu = new OwnMenuRes
                        {
                              Id = childMenu.Id,
                              Label = childMenu.Name,
                        };
                        // 如果子菜单不在指定角色拥有的菜单ID列表中，则禁用该子菜单
                        if (!ownMenuListIds.Contains(childMenu.Id))
                        {
                              childOwnMenu.Disabled = true;
                        }
                        // 将子菜单添加到当前菜单的子菜单列表中
                        ownMenu.Children.Add(childOwnMenu);
                        // 递归调用构建子菜单树
                        BuildMenuTree(childMenu, childOwnMenu, allMenuList, ownMenuListIds);
                  }
            }

      }
}
