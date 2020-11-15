using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Shared
{
   public class OrgSelectOptions 
    {
        /// <summary>
        /// 选中
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public Guid OrgId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string OrgName { get; set; }
    }
}
