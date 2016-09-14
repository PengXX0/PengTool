using System;
using System.Linq;

namespace Pengxsoft.DbToModel
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = BusinessUtilities.GetDbStructures();
            var tables = list.GroupBy(s => s.TableName);
            var value = args.Length == 0 ? 1 : (Convert.ToInt32(args[0]) == 0 ? 0 : 1);
            String templateString = "";
            switch (value)
            {
                case 0:
                    foreach (var item in tables)
                    {
                        Print("正在生成表" + item.Key);
                        var fieldList = list.Where(s => s.TableName == item.Key).ToList();
                        templateString += BusinessUtilities.ModelTemplate(fieldList);
                    }
                    templateString = BusinessUtilities.FrameTemplate(templateString);
                    BusinessUtilities.GenerateTemplate(templateString);
                    break;
                case 1:
                    foreach (var item in tables)
                    {
                        Print("正在生成表" + item.Key);
                        var fieldList = list.Where(s => s.TableName == item.Key).ToList();
                        templateString = BusinessUtilities.ModelTemplate(fieldList);
                        BusinessUtilities.GenerateTemplate(BusinessUtilities.FrameTemplate(templateString), item.Key);
                    }
                    break;
            }
        }

        public static void Print(String message)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + " " + message);
        }
    }
}
