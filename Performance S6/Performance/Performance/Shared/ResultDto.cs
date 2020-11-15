using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Performance.Shared
{
    public class ResultMsg
    {
        public ResultMsg()
        {
        }

        public ResultMsg(bool isSuccess, string msg)
        {
            this.Msg = msg;
            this.IsSuccess = isSuccess;
        }

        public ResultMsg(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
        }

        public string Msg { get; set; } = null;

        public bool IsSuccess { get; set; }
    }

    public class ResultData<T> : ResultMsg
    {
        public ResultData() : base()
        {
          
        }

        public ResultData(T data) : base(true)
        {
            this.Data = data;
        }

        public ResultData(T data, bool isSuccess, string msg) : base(isSuccess, msg)
        {
            this.Data = data;
        }

        public ResultData(bool isSuccess, string msg) : base(isSuccess, msg)
        {
#pragma warning disable CS8601 // 可能的 null 引用赋值。
            Data = default;
#pragma warning restore CS8601 // 可能的 null 引用赋值。
        }
        public T Data { get; set; }

    }

    public class ResultDataSet<T> : ResultMsg
    {
        public ResultDataSet() : base()
        {
            DataSet = null;
        }

        public ResultDataSet(List<T> dataSet, int total) : base(true)
        {
            this.DataSet = dataSet;
            this.Total = total;
        }

        public ResultDataSet(List<T> dataSet) : base(true)
        {
            this.DataSet = dataSet;
            this.Total = dataSet.Count();
        }

        public List<T> DataSet { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }
    }

}
