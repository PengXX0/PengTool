using System;
using System.Web;

namespace DotNet.Utilities
{
    public class CookieManager
    {
        private static HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
        public CookieManager() { }

        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="key">cookiename</param>
        /// <returns></returns>
        public static string Get(string key)
        {
            HttpCookie cookie = cookies[key];
            return cookie == null ? String.Empty : cookie.Value;
        }
        /// <summary>
        /// 添加一个Cookie（24小时过期）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Add(string key, string value)
        {
            Add(key, value, DateTime.Now.AddDays(1.0));
        }
        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="key">cookie名</param>
        /// <param name="value">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void Add(string key, string value, DateTime expires)
        {
            cookies.Add(new HttpCookie(key) { Value = value, Expires = expires });
        }

        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="key">key</param>
        public static void Remove(string key)
        {
            HttpCookie cookie = cookies[key];
            if (cookie != null) { return; }
            cookie.Expires = DateTime.Now.AddYears(-3);
            cookies.Add(cookie);
        }
    }
}
