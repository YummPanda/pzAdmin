using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pzAdmin.Common.Res;
using pzAdmin.Service.MenuServ;

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
            [HttpGet]
            public async Task<List<OwnMenuRes>> GetOwnMenu(int roleId)
            {
                  return await menuService.GetOwnMenuListAsync(roleId);
            }
      }
}
