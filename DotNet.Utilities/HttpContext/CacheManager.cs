using System;
using System.Web;
using System.Collections;
using System.Web.Caching;

namespace DotNet.Utilities
{
    public class CacheManager
    {
        private static readonly Cache Cache = HttpRuntime.Cache;

        public CacheManager() { }

        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="key">键</param>
        public static object Get(string key)
        {
            return Cache[key];
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void Set(string key, object value)
        {
            Cache.Insert(key, value);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void Set(string key, object value, TimeSpan timeout)
        {
            Cache.Insert(key, value, null, DateTime.MaxValue, timeout, CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void Set(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            Cache.Insert(key, value, null, absoluteExpiration, slidingExpiration);
        }


        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAll()
        {
            var cacheDic = Cache.GetEnumerator();
            while (cacheDic.MoveNext()) { Cache.Remove(cacheDic.Key.ToString()); }
        }
    }
}