using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Shared
{
    public class SetImportantReq
    {
        public Guid TaskId { get; set; }

        public bool IsImportant { get; set; }
    }
}
