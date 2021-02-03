using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Shared
{
    public class SetFinishReq
    {
        public Guid TaskId { get; set; }

        public bool IsFinish { get; set; }
    }
}
