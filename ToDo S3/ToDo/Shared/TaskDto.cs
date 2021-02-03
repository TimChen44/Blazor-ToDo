using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ToDo.Shared
{
    public class TaskDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName("标题")]
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        public string Description { get; set; }

        /// <summary>
        /// 计划日期
        /// </summary>
        [DisplayName("计划日期")]
        public DateTime PlanTime { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        [DisplayName("截止日期")]
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// 是否重要
        /// </summary>
        [DisplayName("重要")]
        public bool IsImportant { get; set; }

        /// <summary>
        /// 完成
        /// </summary>
        [DisplayName("完成")]
        public bool IsFinish { get; set; }

    }
}
