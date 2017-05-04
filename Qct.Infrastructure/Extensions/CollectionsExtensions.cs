using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Qct.Infrastructure.Extensions
{
    public static class CollectionsExtensions
    {
        /// <summary>
        /// 遍历集合
        /// </summary>
        /// <typeparam name="T">集合项的类型</typeparam>
        /// <param name="col">集合</param>
        /// <param name="handler">每项处理句柄</param>
        public static void Each<T>(this IEnumerable<T> col, Func<T, bool> handler)
        {
            if (col == null) return;
            foreach (T local in col)
            {
                if (!handler(local))
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 遍历集合
        /// </summary>
        /// <typeparam name="T">集合项的类型</typeparam>
        /// <param name="col">集合</param>
        /// <param name="handler">每项处理句柄</param>
        public static void Each<T>(this IEnumerable<T> col, Action<T> handler)
        {
            if (col == null) return;
            foreach (T local in col)
            {
                handler(local);
            }
        }
        /// <summary>
        /// 将字典添加到指定字典的末尾
        /// </summary>
        /// <typeparam name="TKey">字典键类型</typeparam>
        /// <typeparam name="TValue">字典值类型</typeparam>
        /// <param name="dicts">指定接收字典</param>
        /// <param name="addDicts">要添加的字典</param>
        /// <returns>返回接收字典</returns>
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dicts, Dictionary<TKey, TValue> addDicts)
        {
            if (dicts == null)
            {
                dicts = new Dictionary<TKey, TValue>();
            }
            if (addDicts == null) return dicts;
            foreach (var de in addDicts)
            {
                if (!dicts.ContainsKey(de.Key))
                    dicts.Add(de.Key, de.Value);
            }
            return dicts;
        }
        /// <summary>
        /// Linq动态排序(反射.排序名称必须与Model一致)
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="source">要排序的数据源</param>
        /// <param name="value">排序依据（加空格）排序方式</param>
        /// <returns>IOrderedQueryable</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string fieldName, bool isAsc)
        {
            if (fieldName.IsNullOrEmpty()) return (IOrderedQueryable<T>)source;
            string Name = isAsc ? "OrderBy" : "OrderByDescending";
            return source.ExecuteMethod(fieldName, Name);
        }
        /// <summary>
        /// Linq动态排序再排序(反射.排序名称必须与Model一致)
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="source">要排序的数据源</param>
        /// <param name="value">排序依据（加空格）排序方式</param>
        /// <returns>IOrderedQueryable</returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string fieldName, bool isAsc)
        {
            if (fieldName.IsNullOrEmpty()) return source;
            string Name = isAsc ? "ThenBy" : "ThenByDescending";
            return source.ExecuteMethod(fieldName, Name);
        }
        /// <summary>
        /// 反射操作集合方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ExecuteMethod<T>(this IQueryable<T> source, string property, string methodName)
        {
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "a");
            PropertyInfo pi = type.GetProperty(property);
            Expression expr = Expression.Property(arg, pi);
            type = pi.PropertyType;
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            object result = typeof(Queryable).GetMethods().Single(
            a => a.Name == methodName
            && a.IsGenericMethodDefinition
            && a.GetGenericArguments().Length == 2
            && a.GetParameters().Length == 2).MakeGenericMethod(typeof(T), type).Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

        /// <summary>
        /// 去除重重
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector">p=>new { p.Id, p.Name }</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        /// <summary>
        /// 转成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {
            if (list == null || !list.Any()) return null;
            //PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            //var type = typeof(T);
            var type = list.FirstOrDefault().GetType();
            var properties = type.GetProperties();
            DataTable dt = new DataTable();
            for (int i = 0; i < properties.Count(); i++)
            {
                var property = properties[i];
                var type1 = property.PropertyType;
                if (type1.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    type1 = property.PropertyType.BaseType;
                dt.Columns.Add(property.Name, type1);
            }
            object[] values = new object[properties.Count()];
            foreach (T item in list)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
        
        /// <summary>
        /// 克隆列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> ToClone<T>(this IEnumerable<T> list) where T : class, new()
        {
            var ls = new List<T>();
            foreach (var t in list)
            {
                T obj = new T();
                t.ToCopyProperty(obj);
                ls.Add(obj);
            }
            return ls;
        }
        /// <summary>
        /// 获取字典值，如果key为字符串，则忽略大小写
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dicts"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetValue<Tkey, TValue>(this Dictionary<Tkey, TValue> dicts, Tkey key, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (dicts == null) return default(TValue);
            TValue val = default(TValue);
            if (typeof(Tkey) == typeof(string))
            {
                foreach (var d in dicts)
                {
                    if (string.Equals(d.Key.ToString(), key.ToString(), stringComparison))
                    {
                        val = d.Value;
                        break;
                    }
                }
            }
            else
                dicts.TryGetValue(key, out val);
            return val;
        }
    }
}
