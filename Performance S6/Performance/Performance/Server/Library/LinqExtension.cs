using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Performance.Server
{
    public static class LinqExtension
    {
        /// <summary>
        /// 获得最大值
        /// </summary>
        public static decimal MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal defaults = 0)
        {
            if (!source.Any()) return defaults;
            return source.Max(selector);
        }
        public static int MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector, int defaults = 0)
        {
            if (!source.Any()) return defaults;
            return source.Max(selector);
        }


        /// <summary>
        /// 获得最小值
        /// </summary>
        public static decimal MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal defaults = 0)
        {
            if (!source.Any()) return defaults;
            return source.Min(selector);
        }
        public static int MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector, int defaults = 0)
        {
            if (!source.Any()) return defaults;
            return source.Min(selector);
        }

        /// <summary>
        /// 比价两个集合，获得差异信息
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="predicate">比较委托，第一个参数是源，第二个参数是目标</param>
        /// <returns></returns>
        public static (List<TTarget> insert, List<(TSource source, TTarget target)> update, List<TSource> delete) CompareChange<TSource, TTarget>(this IEnumerable<TSource> source, IEnumerable<TTarget> target, Func<TSource, TTarget, bool> predicate)
        {
            var insert = new List<TTarget>();//新增的
            var update = new List<(TSource source, TTarget target)>();//更新的
            var delete = new List<TSource>();//删除的

            var targetItems = target.ToList();
            foreach (var item in source)
            {
                var targetItem = targetItems.FirstOrDefault(x => predicate(item, x));

                if (targetItem != null)
                {//在目标中找到
                    update.Add((item, targetItem));
                    targetItems.Remove(targetItem);
                }
                else
                {//找不到说明需要删除
                    delete.Add(item);
                }
            }

            insert.AddRange(targetItems);//余下的为新增的

            return (insert, update, delete);

        }
    }
}
