using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Shared
{
   public class OrgTreeDto
    {
        /// <summary>
        /// 机构/部门
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public Guid? ParentId { get; set; }

        public string Name { get; set; }

        public List<OrgTreeDto> Childs { get; set; } = new List<OrgTreeDto>();
    }
}
