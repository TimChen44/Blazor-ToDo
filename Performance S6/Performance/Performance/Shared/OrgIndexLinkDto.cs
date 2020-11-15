using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Shared
{
   public  class OrgIndexLinkDto
    {
        /// <summary>
        /// 机构/部门
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 指标
        /// </summary>
        public Guid IndexLibraryId { get; set; }
    }
}
