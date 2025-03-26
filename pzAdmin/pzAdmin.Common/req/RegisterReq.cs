using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Common.req
{
      public class RegisterReq
      {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string YzmCode { get; set; }
            public string YzmKey { get; set; }
      }
}
