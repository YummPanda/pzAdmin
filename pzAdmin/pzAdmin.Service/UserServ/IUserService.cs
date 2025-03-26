using pzAdmin.Common;
using pzAdmin.Common.req;
using pzAdmin.Common.Res;
using pzAdmin.Model;
using pzAdmin.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Service.UserServ
{
      public interface IUserService : IBaseService<User>
      {
            //注册
            Task<bool> Register(RegisterReq req);
            //登录
            Task<ApiResult<LoginRes>> Login(RegisterReq req);
      }
}
