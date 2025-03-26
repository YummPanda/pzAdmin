using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Common.JWTExtension
{
      public class JwtOptions
      {
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public string Key { get; set; }
            public int ExpireSeconds { get; set; }
      }
}
