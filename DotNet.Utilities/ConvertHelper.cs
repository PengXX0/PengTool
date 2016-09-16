using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNet.Utilities
{
    /// <summary>
    /// 处理数据类型
    /// </summary>    
    public static class ConvertHelper
    {
        #region 各进制数间转换
        /// <summary>
        /// 实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="value">要转换的值,即原值</param>
        /// <param name="from">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="to">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        public static string ConvertBase(string value, int from, int to)
        {
            try
            {
                int intValue = Convert.ToInt32(value, from);  //先转成10进制
                string result = Convert.ToString(intValue, to);  //再转成目标进制
                if (to == 2)
                {
                    int resultLength = result.Length;  //获取二进制的长度
                    switch (resultLength)
                    {
                        case 7:
                            result = "0" + result;
                            break;
                        case 6:
                            result = "00" + result;
                            break;
                        case 5:
                            result = "000" + result;
                            break;
                        case 4:
                            result = "0000" + result;
                            break;
                        case 3:
                            result = "00000" + result;
                            break;
                    }
                }
                return result;
            }
            catch
            {

                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return "0";
            }
        }
        #endregion

        public static DateTime ToTime(this long value)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value).ToLocalTime();
        }


        /// <summary>
        /// 给电话号码截断为星号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static string ToAnonymous(this string mobile)
        {
            //两种分组的写法

            //方法1
            //return Regex.Replace(mobile, @"^(?<a>\d{3})\d{4}(?<c>\d{4})$", "${a}****${c}");

            //方法2
            return Regex.Replace(mobile, @"^(?'a'\d{3})\d{4}(?'c'\d{4})$", "${a}****${c}");





            //return Regex.Replace(mobile, @"^\d{3}(?<value>\d{4})\d{4}$","****",RegexOptions.ExplicitCapture);
            //return new Regex(@"^\d{3}(\d{4})\d{4}$").Replace(mobile, "****");
            //var mc = new Regex(@"^\d{3}(?<value>\d{4})\d{4}$").Matches(mobile);
            //var ss = new List<String>();
            //for (int i = 0; i < mc.Count; i++)
            //{
            //    ss.Add(mc[i].Groups["value"].Value);
            //}
            //return String.Join(",", ss);
        }

        #region DataTable与List互转
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class,new()
        {
            //创建一个属性的列表
            var prlist = new List<PropertyInfo>();
            //获取TResult的类型实例  反射的入口
            var t = typeof(T);
            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表 
            Array.ForEach(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //创建返回的集合
            var oblist = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                T ob = new T();
                //找到对应的数据  并赋值
                prlist.ForEach(p => { if (row[p.Name] != DBNull.Value) p.SetValue(ob, row[p.Name], null); });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }

        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> value) where T : class
        {
            //创建属性的集合
            var pList = new List<PropertyInfo>();
            //获得反射的入口
            var type = typeof(T);
            var dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列
            Array.ForEach(type.GetProperties(), p =>
            {
                pList.Add(p);
                dt.Columns.Add(p.Name);
            });
            //dt.Columns.Add(p.Name, p.PropertyType);
            foreach (var item in value)
            {
                //创建一个DataRow实例
                var row = dt.NewRow();
                //给row 赋值
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable
                dt.Rows.Add(row);
            }
            return dt;
        }
        #endregion
    }
}
