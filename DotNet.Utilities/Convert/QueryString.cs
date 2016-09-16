using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNet.Utilities
{
    /// <summary>
    /// QueryString
    /// </summary>
    public static class QueryString
    {
        /// <summary>
        /// QueryString转Dictionary
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static Dictionary<String, Object> ToDictionary(string queryString)
        {
            return queryString.Split('&').Select(item => item.Split('=')).ToDictionary<String[], String, Object>(args => args[0], args => args[1]);
        }

        /// <summary>
        /// Dictionary转QueryString
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string ToQueryString(this Dictionary<String, String> dic)
        {
            return dic.Aggregate("", (current, i) => current + (i.Key + "=" + i.Value + "&")).TrimEnd('&');
        }

        /// <summary>
        /// Model转QueryString
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String ToQueryString(this Object obj)
        {
            var pi = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var list = from p in pi
                       let mi = p.GetGetMethod()
                       where mi != null && mi.IsPublic
                       let value = mi.Invoke(obj, null) ?? ""
                       select p.Name + "=" + value;
            return String.Join("&", list);
        }
    }
}
