using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Shared
{
    public class IndexLibraryDto
    {
        /// <summary>
        /// 指标
        /// </summary>
        [Key]
        public Guid IndexLibraryId { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("指标名称")]
        public string Name { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("指标范围")]
        public string Scope { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("指标类别")]
        public string Type { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("单位")]
        public string Unit { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("指标定义")]
        public string Definition { get; set; }
        [StringLength(100)]
        [DisplayName("备注")]
        public string Remark { get; set; }
        [DisplayName("是否启用")]
        public bool IsEnabled { get; set; }
    }
}
