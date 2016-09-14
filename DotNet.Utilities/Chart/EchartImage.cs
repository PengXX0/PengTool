using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DotNet.Utilities;
using DotNet.Utilities.Properties;

namespace Peng.Load
{
    /// <summary>
    /// 通过百度Echart插件，在网页上生成各种统计图
    /// 相关说明：http://echarts.baidu.com/api.html
    /// </summary>
    public static class EchartImage
    {
        private static string mFileName = string.Empty;
        private static string mJson = string.Empty;
        private static string filePath = string.Empty;
        private static int mWidth = 600;
        private static int mHeight = 400;

        /// <summary>
        /// 开始运行
        /// </summary>
        /// <param name="fileName">文件路径，例如：  C:\FileName\</param>
        /// <param name="json">生成图表用的json数据</param>
        /// <param name="width">生成图像的宽度 单位：px</param>
        /// <param name="height">生成图像的高度 单位：px</param>
        /// <returns></returns>
        public static String Run(string json, string fileName, int? width = null, int? height = null)
        {
            mFileName = fileName; mJson = json;
            mWidth = width ?? mWidth;
            mHeight = height ?? mHeight;
            var th = new Thread(LoadPage);
            th.SetApartmentState(ApartmentState.STA);
            th.Start(); th.Join();
            return filePath;
        }

        private static void LoadPage()
        {
            var browser = new WebBrowser { Url = new Uri("about:blank") };
            var html = String.Format(Resource.echartHtml, mWidth, mHeight, Resource.echartsJs);
            browser.Document.Write(html);
            RenderImage(browser);
        }

        private static void RenderImage(WebBrowser browser)
        {
            browser.Document.InvokeScript("run", new Object[] { mJson });
            var imgBase64 = browser.Document.GetElementById("i").GetAttribute("value");
            var pattern = @"^data:image\/+(gif|png|jpg|jpeg+);base64,([a-zA-Z0-9,/,+,=]+)$";
            var match = new Regex(pattern, RegexOptions.IgnoreCase).Match(imgBase64.Replace(" ", "+"));
            if (!match.Success) { throw new Exception("字符串格式不正确！"); }
            var img = Image.FromStream(new MemoryStream(Convert.FromBase64String(match.Groups[2].Value.Replace(" ", "+"))));
            var dirpath = mFileName.Substring(0, mFileName.LastIndexOf('/'));
            if (!Directory.Exists(dirpath)) { Directory.CreateDirectory(dirpath); }
            filePath = Path.GetFullPath(dirpath + "/" + Guid.NewGuid().ToString("N") + "." + match.Groups[1]);
            img.Save(filePath);
        }

        #region 加载文件的方式
        //private static void LoadPageOld()
        //{
        //    var url = Uri.UriSchemeFile + Uri.SchemeDelimiter + Path.GetFullPath(Environment.CurrentDirectory + "../../../echarts.html").Replace(Path.DirectorySeparatorChar, '/');
        //    var browser = new WebBrowser();
        //    browser.Navigate(url);
        //    browser.DocumentCompleted += RenderImageOld;
        //    while (browser.ReadyState != WebBrowserReadyState.Complete)
        //    { Application.DoEvents(); } browser.Dispose();
        //}

        //private static void RenderImageOld(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
        //    var browser = (WebBrowser)sender;
        //    browser.Document.InvokeScript("run", new Object[] { mJson });
        //    var imgBase64 = browser.Document.GetElementById("i").GetAttribute("value");
        //    var pattern = @"^data:image\/+(gif|png|jpg|jpeg+);base64,([a-zA-Z0-9,/,+,=,\s]+)$";
        //    var match = new Regex(pattern, RegexOptions.IgnoreCase).Match(imgBase64);
        //    if (!match.Success) { throw new Exception("字符串格式不正确！"); }
        //    var img = Image.FromStream(new MemoryStream(Convert.FromBase64String(match.Groups[2].Value.Replace(" ", "+"))));
        //    var dirpath = mFileName.Substring(0, mFileName.LastIndexOf('/'));
        //    if (!Directory.Exists(dirpath)) { Directory.CreateDirectory(dirpath); }
        //    filePath = Path.GetFullPath(dirpath + "/" + Guid.NewGuid().ToString("N") + "." + match.Groups[1]);
        //    img.Save(filePath);
        //}
        #endregion
    }
}
