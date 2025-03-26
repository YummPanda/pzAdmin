using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pzAdmin.Common;
using pzAdmin.Common.CachExtension;
using pzAdmin.Common.req;
using pzAdmin.Common.Res;
using pzAdmin.Service.UserServ;

namespace pzAdmin.WebAPI.Controllers
{
      [Route("api/[controller]/[action]")]
      [ApiController]
      public class UserController : ControllerBase
      {
            private readonly IMemoryCacheHelper memoryCacheHelper;
            private readonly IUserService userService;

            public UserController(IMemoryCacheHelper memoryCacheHelper, IUserService userService)
            {
                  this.memoryCacheHelper = memoryCacheHelper;
                  this.userService = userService;
            }


            /// <summary>
            /// 获取验证码图片
            /// </summary>
            [HttpGet]
            public async Task<ApiResult<YzmRes>> GetYzmImage()
            {
                  var (code, bytes) = Utils.CreateYzmImage(4);
                  var YzmKey = new Guid().ToString();
                  //保存验证码到缓存
                  memoryCacheHelper.Set(YzmKey, code.ToLower());
                  var dataRep = new YzmRes()
                  {
                        YzmKey = YzmKey,
                        YzmBytes = bytes
                  };
                  return new ApiResult<YzmRes>()
                  {
                        Success = true,
                        Data = dataRep
                  };
            }
            /// <summary>
            /// 注册
            /// </summary>
            [HttpPost]
            public async Task<ApiResult<string>> Register([FromBody] RegisterReq req)
            {
                  //验证验证码
                  memoryCacheHelper.TryGetValue(req.YzmKey, out string code);
                  if (code != req.YzmCode.ToLower())
                  {
                        return new ApiResult<string>()
                        {
                              Success = false,
                              Message = "验证码错误"
                        };
                  }
                  //注册
                  //...
                  var isSuccess = await userService.Register(req);
                  if (isSuccess)
                  {
                        return new ApiResult<string>()
                        {
                              Success = true,
                              Message = "注册成功"
                        };

                  }
                  else
                  {
                        return new ApiResult<string>()
                        {
                              Success = false,
                              Message = "注册失败"
                        };
                  }

            }

            /// <summary>
            /// 登录
            /// </summary>
            [HttpPost]
            public async Task<ApiResult<LoginRes>> Login([FromBody] RegisterReq req)
            {
                  return await userService.Login(req);
            }



      }
}
