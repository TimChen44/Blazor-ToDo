using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Shared
{
  public   class SetStarReq
    {
        public Guid TaskId { get; set; }

        public bool IsStar { get; set; }
    }
}
