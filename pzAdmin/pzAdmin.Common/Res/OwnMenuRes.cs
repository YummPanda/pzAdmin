using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Common.Res
{
      public class OwnMenuRes
      {
            public int Id { get; set; }
            public string Label { get; set; }
            public bool Disabled { get; set; }
            public List<OwnMenuRes> Children { get; set; }= new List<OwnMenuRes>();

      }
}
