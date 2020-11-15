using Microsoft.EntityFrameworkCore;
using Performance.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Performance.Server
{
    public static class IQueryableExtension
    {
        #region 条件

        /// <summary>
        /// 追加条件，如果值为空时不追加
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbQuery"></param>
        /// <param name="value"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> AddWhere<TEntity>(this IQueryable<TEntity> dbQuery, string value, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(value))
                return dbQuery;
            else
                return dbQuery.Where(predicate);
        }

        public static IQueryable<TEntity> AddWhere<TEntity>(this IQueryable<TEntity> dbQuery, decimal? value, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            if (value.HasValue == false)
                return dbQuery;
            else
                return dbQuery.Where(predicate);
        }

        public static IQueryable<TEntity> AddWhere<TEntity>(this IQueryable<TEntity> dbQuery, int? value, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            if (value.HasValue == false)
                return dbQuery;
            else
                return dbQuery.Where(predicate);
        }

        public static IQueryable<TEntity> AddWhere<TEntity>(this IQueryable<TEntity> dbQuery, Guid? value, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            if (value.HasValue == false)
                return dbQuery;
            else
                return dbQuery.Where(predicate);
        }

        public static IQueryable<TEntity> AddWhere<TEntity>(this IQueryable<TEntity> dbQuery, DateTime? value, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            if (value.HasValue == false)
                return dbQuery;
            else
                return dbQuery.Where(predicate);
        }

        public static IQueryable<TEntity> AddWhere<TEntity>(this IQueryable<TEntity> dbQuery, bool? value, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            if (value.HasValue == false)
                return dbQuery;
            else
                return dbQuery.Where(predicate);
        }

        public static IQueryable<TEntity> AddWhere<TEntity>(this IQueryable<TEntity> dbQuery, string[] value, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            if (value == null || value.Length == 0)
                return dbQuery;
            else
                return dbQuery.Where(predicate);
        }

        //时间范围检索
        public static IQueryable<TEntity> AddWhere<TEntity>(this IQueryable<TEntity> dbQuery, DateTime[] value, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            if (value == null)
                return dbQuery;
            else
                return dbQuery.Where(predicate);
        }

        /// <summary>
        /// 将文本拆解成Like用的文本集合，文本默认小写
        /// </summary>
        /// <param name="queryText"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> SplitToLike(this string queryText, params char[]? separator)
        {
            return queryText.ToLower().Split(separator, StringSplitOptions.RemoveEmptyEntries).Distinct().Select(x => $"%{x}%").ToList();
        }

        #endregion

        #region 排序

        /// <summary>
        /// 根据排序字段追加排序
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="dbQuery"></param>
        /// <param name="sortField"></param>
        /// <param name="sortOrder"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> AddOrderBy<TEntity, TKey>(this IQueryable<TEntity> dbQuery, string sortOrder, Expression<Func<TEntity, TKey>> keySelector) where TEntity : class
        {
            if (sortOrder != null && sortOrder.StartsWith("desc") == true)
            {
                return dbQuery.OrderByDescending(keySelector);
            }
            else
            {
                return dbQuery.OrderBy(keySelector);
            }
        }

        #endregion

        #region 分页

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbQuery"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> Page<TEntity>(this IQueryable<TEntity> dbQuery, int pageIndex, int pageSize) where TEntity : class
        {
            return dbQuery.Skip(pageIndex * pageSize).Take(pageSize);
        }

        public static IQueryable<TEntity> Page<TEntity>(this IQueryable<TEntity> dbQuery, QueryDto queryParams) where TEntity : class
        {
            return dbQuery.Page(queryParams.PageIndex, queryParams.PageSize);
        }

        /// <summary>
        /// 分页，用于阿里Ant Design
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbQuery"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> PageForAnt<TEntity>(this IQueryable<TEntity> dbQuery, int pageIndex, int pageSize) where TEntity : class
        {
            if (pageIndex != 0) pageIndex--;//跳过的数量是页码减1
            return dbQuery.Skip(pageIndex * pageSize).Take(pageSize);
        }

        public static IQueryable<TEntity> PageForAnt<TEntity>(this IQueryable<TEntity> dbQuery, QueryDto queryParams) where TEntity : class
        {
            return dbQuery.PageForAnt(queryParams.PageIndex, queryParams.PageSize);
        }

        #endregion
    }
}
