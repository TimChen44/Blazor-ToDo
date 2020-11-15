using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performance.Shared
{
   public  class AchievementEditDto
    {
        public Guid AchievementId { get; set; }
        /// <summary>
        /// 指标
        /// </summary>
        public Guid IndexLibraryId { get; set; }
        /// <summary>
        /// 指标名称
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// 机构/部门
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 目标值
        /// </summary>
        public decimal TargetValue { get; set; }
        /// <summary>
        /// 实际值
        /// </summary>
        public decimal? ActualValue { get; set; }
        /// <summary>
        /// 保障值
        /// </summary>
        public decimal GuaranteedValue { get; set; }
        /// <summary>
        /// 挑战值
        /// </summary>
        public decimal ChallengeValue { get; set; }
    }
}
