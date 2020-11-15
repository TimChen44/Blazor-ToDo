using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Shared
{
   public class IndexLibraryQueryDto: QueryDto
    {
        [DisplayName("指标名称")]
        public string Name { get; set; }
        [DisplayName("指标范围")]
        public string Scope { get; set; }

        [DisplayName("指标类别")]
        public string Type { get; set; }
        [DisplayName("指标定义")]
        public string Definition { get; set; }
    }
}
