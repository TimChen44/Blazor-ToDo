using System;
using System.Collections.Generic;
using System.Text;

namespace Performance.Shared
{
    /// <summary>
    /// 包含Value和Text的选择对象
    /// </summary>
    public class SelectOptionCore
    {
        public SelectOptionCore() { }

        public SelectOptionCore(string value, string text)
        {
            this.Value = value;
            this.Text = text;
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text { get; set; }


        public override string ToString()
        {
            return Text;
        }
    }

}