using Microsoft.Extensions.Options;
using pzAdmin.Common;
using pzAdmin.Common.JWTExtension;
using pzAdmin.Common.req;
using pzAdmin.Common.Res;
using pzAdmin.Model;
using pzAdmin.Repository.Base;
using pzAdmin.Repository.UserRep;
using pzAdmin.Service.Base;
using System.Security.Claims;

namespace pzAdmin.Service.UserServ
{
      public class UserService : BaseService<User>, IUserService
      {
            private readonly IUserRepository userRepository;
            private readonly IOptions<JwtOptions> jwtOptions;

            public UserService(IBaseRepository<User> baseRepository, IUserRepository userRepository, IOptions<JwtOptions> jwtOptions) : base(baseRepository)
            {
                  this.userRepository = userRepository;
                  this.jwtOptions = jwtOptions;
            }
            //注册
            public async Task<bool> Register(RegisterReq req)
            {
                  var newUser = new User()
                  {
                        Id = Guid.NewGuid(),
                        Name = req.UserName,
                        Password = req.Password,
                        CreateTime = DateTime.Now,

                  };
                  return await userRepository.Add(newUser);
            }

            //登录
            public async Task<ApiResult<LoginRes>> Login(RegisterReq req)
            {
                  var user = await userRepository.getFirstOrDefault(u=>u.Name==req.UserName);
                  var res=new ApiResult<LoginRes>();
                  if (user == null)
                  {
                        res.Success = false;
                        res.Message = "用户名不存在";
                        return res;
                  }
                  if (user.Password!= req.Password)
                  {
                        res.Success = false;
                        res.Message = "密码错误";
                        return res;
                  }
                  var claims = new List<Claim>();
                  claims.Add(new Claim(ClaimTypes.Name, user.Name));
                  claims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));

                  string token = JwtHelper.BuildJwtToken(claims, jwtOptions.Value);
                  res.Data = new LoginRes() { Token = token ,UserInfo=user};
                  res.Success = true;
                  res.Message = "登录成功";
                  return res;
            }
      }
}
