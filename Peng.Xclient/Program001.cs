using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Peng.Xclient
{
    class Program001
    {
        static void Main001(string[] args)
        {
            //测试数据
            var t1 = new List<Table1> {
            new Table1{Name="水果",NetSales=3124124,GiftCards=134124},
            new Table1{Name="蔬菜",NetSales=123,GiftCards=1000}  
            };

            //通过反射取出表头
            var listCol = t1.First().GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(s => s.Name).ToList();
            var firstRow = t1.Select(s => s.Name).ToList();
            firstRow.Insert(0, "品类报表");
            var list = new List<Array> { firstRow.ToArray() };

            //取出数据
            for (var i = 1; i < listCol.Count(); i++)
            {
                var tem = new List<Object> { listCol[i] };
                tem.AddRange(t1.Select(item => ToArray(item)[i]));
                list.Add(tem.ToArray());
            }
            Console.WriteLine(JsonConvert.SerializeObject(list));
            Console.ReadKey();
        }

        public static Object[] ToArray(Object obj)
        {
            var pi = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return (from p in pi select p.GetGetMethod() into mi let value = mi.Invoke(obj, new Object[] { }) ?? "" where mi != null && mi.IsPublic select value).ToArray();
        }

        /// <summary>
        /// 行转列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<Array> RowToColumn<T>(List<T> list) where T : class,new()
        {
            var properties = list.First().GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var columnList = properties.Select(s => s.Name).ToList();
            var datalist = new List<Array>();
            for (var i = 1; i < columnList.Count(); i++)
            {
                var tem = new List<Object> { columnList[i] };
                tem.AddRange(list.Select(item => ((from p in properties
                                                   select p.GetGetMethod()
                                                       into mi
                                                       let value = mi.Invoke(item, new Object[] { }) ?? ""
                                                       where mi != null && mi.IsPublic
                                                       select value).ToList())[i]));
                datalist.Add(tem.ToArray());
            }
            return datalist;
        }
    }

    public class Table1
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 销售净额
        /// </summary>
        public int NetSales { get; set; }
        /// <summary>
        /// 礼品卡
        /// </summary>
        public int GiftCards { get; set; }
    }
}
