using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Shared
{
    public class GetSearchReq
    {
        public string QueryTitle { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public List<SortFieldName> Sorts { get; set; }
    }

    public class SortFieldName
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 排序方向
        /// </summary>
        public string SortOrder { get; set; }

    }

}
