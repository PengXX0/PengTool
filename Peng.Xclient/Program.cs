using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pengxsoft.Xclient
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://hotels.ctrip.com/Domestic/tool/AjaxHotelCommentList.aspx?MasterHotelID=2083684&hotel=2083684&property=0&card=0&cardpos=0&NewOpenCount=0&AutoExpiredCount=0&RecordCount=959&OpenDate=2015-10-01&abForComLabel=False&viewVersion=c&productcode=&currentPage=1&commentType=0&writtingId=0&contyped=0&eleven=KOorOhLZhyVL86vPdZn%2F5w%3D%3D&callback=CASrMkQvQjxdbGhIK&_=1467023426880";
            var cookie = "_abtest_userid=cd19bbff-6fb3-4deb-9512-59d643d24cfe; returncash=1; HotelDomesticVisitedHotels1=2083684=0,0,4.6,957,/fd/hotel/g4/M09/08/B7/CggYHFZFlDCAcC6hAB_pS8e9GFw255.jpg,; _jzqco=%7C%7C%7C%7C%7C1.2112886263.1462254212957.1467022980061.1467023057701.1467022980061.1467023057701.0.0.0.14.14; __zpspc=9.3.1467022032.1467023057.5%234%7C%7C%7C%7C%7C%23; _ga=GA1.2.1353405707.1462254213; _bfa=1.1462254209524.1vfeoa.1.1467013502673.1467022029818.3.16; _bfs=1.6; _bfi=p1%3D102003992%26p2%3D102003992%26v1%3D16%26v2%3D15";

            var html = SendGetHttpRequest(url, cookie, "http://hotels.ctrip.com/hotel/dianping/2083684.html");

            var list = GetValueListByTag(html, "<div class=\"bar_score\">", "</div>", false);
            foreach (var item in list)
            {
                Console.Write(item);
            }

            Console.Write(html);
            Console.ReadKey();
        }

        public static string SendGetHttpRequest(string url, string cookie, string referer)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Host = "hotels.ctrip.com";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36";
            request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            request.Headers.Add("Cookie", cookie);
            request.Referer = referer;
            string result = string.Empty;
            using (var response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                }
            }
            return result;
        }

        public static string GetValueByTag(string html, string beginTag, string endTag, bool b)
        {
            string str = "";
            string pattern = beginTag + ".*?" + endTag;
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection mc = regex.Matches(html);
            if (mc.Count > 0)
            {
                str = mc[0].ToString();
            }
            if (!b)
            {
                str = str.Replace(beginTag, "");
                str = str.Replace(endTag, "");
            }
            return str;
        }


        public static IList<string> GetValueListByTag(string html, string beginTag, string endTag, bool b)
        {
            IList<string> list = new List<string>();
            string pattern = beginTag + ".*?" + endTag;
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection mc = regex.Matches(html);
            for (int i = 0; i < mc.Count; i++)
            {
                string s = mc[i].ToString();
                if (!b)
                {
                    s = s.Replace(beginTag, "");
                    s = s.Replace(endTag, "");
                }
                list.Add(s);
            }
            return list;
        }
    }
}
