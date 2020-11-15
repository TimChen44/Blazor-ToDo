using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Shared
{
   public class OrgDto
    {
        /// <summary>
        /// 机构/部门
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public Guid? ParentId { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("名称")]
        public string Name { get; set; }
        [StringLength(100)]
        [DisplayName("地址")]
        public string Address { get; set; }
        [StringLength(100)]
        [DisplayName("电话")]
        public string Phone { get; set; }
        [StringLength(100)]
        [DisplayName("描述")]
        public string Description { get; set; }
    }
}
