using System;
using System.Web;
using System.Web.SessionState;

namespace DotNet.Utilities
{
    public class SessionManager
    {
        private static HttpSessionState session = HttpContext.Current.Session;
        public SessionManager()
        {            
        }

        /// <summary>
        /// 添加Session，调动有效期为20分钟
        /// </summary>
        /// <param name="key">Session对象名称</param>
        /// <param name="value">Session值</param>
        public static void Add(string key, string value)
        {
            Add(key, value, 20);
        }

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="key">Session对象名称</param>
        /// <param name="value">Session值</param>
        /// <param name="expires">调动有效期（分钟）</param>
        public static void Add(string key, string value, int expires)
        {
            session[key] = value;
            session.Timeout = expires;
        }


        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="key">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static String Get(string key)
        {
            var _sessoin = session[key];
            return _sessoin == null ? null : _sessoin.ToString();
        }

        /// <summary>
        /// 读取某个Session对象值数组
        /// </summary>
        /// <param name="key">Session对象名称</param>
        /// <returns>Session对象值数组</returns>
        public static String[] GetArray(string key)
        {
            var _session = HttpContext.Current.Session[key];
            return session == null ? null : (String[])_session;
        }

        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="key">Session对象名称</param>
        public static void Remove(string key)
        {
            session.Remove(key);
        }
    }
}