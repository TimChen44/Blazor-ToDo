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
        public string FieldName { get; set; }
        public string SortType { get; set; }

    }

}
