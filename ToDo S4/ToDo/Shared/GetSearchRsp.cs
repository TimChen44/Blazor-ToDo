using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Shared
{
    public class GetSearchRsp
    {
        public List<TaskDto> Data { get; set; }

        public int Total { get; set; }
    }
}
