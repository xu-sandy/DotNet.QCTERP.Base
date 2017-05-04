using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.ISevices
{
    public interface ICacheService
    {
        /// <summary>
        /// 根据Key获取指定对象，并设置是否延迟该对象的过期
        /// </summary>
        /// <typeparam name="T">获取的对象类型</typeparam>
        /// <param name="key">用于查询的Key</param>
        /// <param name="isDelayed">是否延期</param>
        /// <param name="expire">延迟多久过期</param>
        /// <returns>缓存对象</returns>
        T GetObject<T>(string key, bool isDelayed = false, TimeSpan? expire = null);
        /// <summary>
        /// 缓存指定对象，并设置过期时间
        /// </summary>
        /// <typeparam name="T">设置的缓存对象类型</typeparam>
        /// <param name="key">用于设置的Key</param>
        /// <param name="obj">缓存对象</param>
        /// <param name="expire">过期时间</param>
        void SetObject<T>(string key, T obj, TimeSpan? expire = null);
        /// <summary>
        /// 根据Key移出指定对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        void RemoveObject<T>(string key);
    }
}
