using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Shared
{
  public  class IndexLibraryChartDto
    {
        public int Year { get; set; }
        public string OrgName { get; set; }
        public Guid  OrgId { get; set; }
        public decimal ActualValue { get; set; }
    }
}
