using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pzAdmin.Common;
using pzAdmin.Common.Res;
using pzAdmin.Service.MenuServ;
using System.Security.Claims;

namespace pzAdmin.WebAPI.Controllers
{
      [Route("api/[controller]/[action]")]
      [ApiController]
      public class MenuController : ControllerBase
      {
            private readonly IMenuService menuService;

            public MenuController(IMenuService menuService)
            {
                  this.menuService = menuService;
            }

            /// <summary>
            /// 获取当前用户的菜单列表
            /// </summary>
            [HttpGet]
            public async Task<ApiResult<List<OwnMenuRes>>> GetOwnMenu()
            {
                  var res=this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                  if (string.IsNullOrEmpty(res))
                  {
                        return new ApiResult<List<OwnMenuRes>>()
                        {
                              Success = false,
                              Message = "当前用户没有角色信息,请重新登录"
                        };
                  }
                  int roleId = int.Parse(res);
                  return new ApiResult<List<OwnMenuRes>>()
                  {
                        Success = true,
                        Data = await menuService.GetOwnMenuListAsync(roleId),
                        Message = "获取成功"

                  };
            }
      }
}
