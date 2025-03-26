using pzAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Common.Res
{
      public class LoginRes
      {
            public string? Token { get; set; }
            public User? UserInfo { get; set; }
      }
}
