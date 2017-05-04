using Qct.Infrastructure.Attributes;
using Qct.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Qct.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 判断对象是否为空，或者为空字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T obj)
        {
            if (obj == null) return true;
            return string.IsNullOrWhiteSpace(obj.ToString());
        }
        /// <summary>
        /// 属性复制，可通过 ExcludeAttribute 排除不需要复制的属性
        /// </summary>
        /// <typeparam name="TEntity1"></typeparam>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="eny1">源对象</param>
        /// <param name="eny2">目标对象</param>
        /// <param name="exclude">是否排除,否则包含</param>
        /// <param name="props"></param>
        public static void ToCopyProperty<TEntity1, TEntity2>(this TEntity1 eny1, TEntity2 eny2,bool exclude=true, params string[] props) where TEntity1 : class
        {
            if (eny1 == null || eny2 == null) return;
            var curType = eny1.GetType();
            var tarType = eny2.GetType();
            var propertyList = (from x in curType.GetProperties()
                                join y in tarType.GetProperties() on x.Name equals y.Name
                                select new
                                {
                                    cur = x,
                                    tar = y
                                });

            foreach (var p in propertyList)
            {
                try
                {
                    if (props != null && ((exclude && props.Contains(p.tar.Name)) || (!exclude && !props.Contains(p.tar.Name)))) continue;
                    if (!p.tar.CanWrite ||
                        (p.tar.PropertyType.IsGenericType && !p.tar.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))) continue;
                    var attrs = p.tar.GetCustomAttributes(typeof(ExcludeAttribute), false);
                    if (attrs.Length > 0) continue;
                    p.tar.SetValue(eny2, p.cur.GetValue(eny1, null), null);
                }
                catch (Exception) { continue; }
            }
        }
        

        /// <summary>
        /// 对于两个相同类的属性进行复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static void CopyProperty<T>(this T target, T source)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            Func<PropertyInfo, bool> filter = p => p.CanRead && p.CanWrite;
            var type = source.GetType();

            var sourceProperties = type.GetProperties(flags).Where(filter);
            var targetProperties = type.GetProperties(flags).Where(filter);

            foreach (var property in targetProperties)
            {
                var s = sourceProperties.SingleOrDefault(p => p.Name.Equals(property.Name)
                         && property.DeclaringType.IsAssignableFrom(p.DeclaringType));
                if (s != null)
                {
                    property.SetValue(target, s.GetValue(source, null), null);
                }
            }
        }

        /// <summary>
        /// 获取一个类指定的属性值
        /// </summary>
        /// <param name="info">object对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public static object GetPropertyValue(this object info, string propertyName)
        {
            if (info == null) return null;
            Type t = info.GetType();
            IEnumerable<PropertyInfo> property = from pi in t.GetProperties() where pi.Name.ToLower() == propertyName.ToLower() select pi;
            return property.Any() ? property.First().GetValue(info, null) : null;
        }
        /// <summary>
        /// 为空抛出异常
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="errMess"></param>
        public static TEntity IsNullThrow<TEntity>(this TEntity obj, string errMess = "传入参数不正确!") where TEntity : class
        {
            if (obj.IsNullOrEmpty()) throw new DataException(errMess);
            return obj;
        }
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum enumValue)
        {
            string str = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            DescriptionAttribute da = (DescriptionAttribute)objs[0];
            return da.Description;
        }
        /// <summary>
        /// 转换成需要类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ToType<T>(this object obj)
        {
            try
            {
                var type = typeof(T);
                if (obj.IsNullOrEmpty())
                    return default(T);
                else if (!type.IsGenericType)
                    return (T)Convert.ChangeType(obj, type);
                else if (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    return (T)Convert.ChangeType(obj, Nullable.GetUnderlyingType(type));
                return default(T);
            }
            catch
            {
                return default(T);
            };

        }
        /// <summary>
        /// 逗号隔开转成需要数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static T[] ToTypeArray<T>(this string str, char separator = ',')
        {
            if (str.IsNullOrEmpty()) return new T[] { };
            return str.Split(separator).Where(o => !o.IsNullOrEmpty()).Select(o => o.ToType<T>()).Distinct().ToArray();
        }

        /// <summary>
        /// 自动转化整型
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static string ToAutoString(this decimal dec, int precision = 2)
        {
            var val = dec.ToString("f" + precision);
            var digit = val.Substring(val.IndexOf(".") + 1);
            if (int.Parse(digit) > 0)
            {
                while (true)
                {
                    if (!val.EndsWith("0")) break;
                    val = val.Remove(val.LastIndexOf("0"), 1);
                }
                return val;
            }
            return dec.ToString("f0");
        }
        /// <summary>
        /// 去除空格（NULL不引发异常）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToTrim(this string str)
        {
            if (str == null) return "";
            return str.Trim();
        }
    }
}
