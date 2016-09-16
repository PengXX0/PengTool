using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DotNet.Utilities
{
    /// <summary>
    /// 根据url保存网页截图
    /// </summary>
    public class WebsiteToImage
    {
        private Bitmap mBitmap;
        private string mUrl;
        private string mFileName = string.Empty;
        public WebsiteToImage(string url)
        {
            // Without file 
            mUrl = url;
        }
        public WebsiteToImage(string url, string fileName)
        {
            // With file 
            mUrl = url;
            mFileName = fileName;
        }
        public Bitmap Generate()
        {
            var mThread = new Thread(_Generate);
            mThread.SetApartmentState(ApartmentState.STA);
            mThread.Start(); mThread.Join();
            return mBitmap;
        }
        private void _Generate()
        {
            var browser = new WebBrowser { ScrollBarsEnabled = false };
            browser.Navigate(mUrl);
            browser.DocumentCompleted += WebBrowserDocumentCompleted;
            while (browser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            browser.Dispose();
        }
        private void WebBrowserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Capture 
            var browser = (WebBrowser)sender;
            //browser.ClientSize = new Size(browser.Document.Body.ScrollRectangle.Width, browser.Document.Body.ScrollRectangle.Bottom);
            browser.ClientSize = new Size(1190, browser.Document.Body.ScrollRectangle.Bottom);
            browser.ScrollBarsEnabled = false;
            mBitmap = new Bitmap(browser.Document.Body.ScrollRectangle.Width, browser.Document.Body.ScrollRectangle.Bottom);
            browser.BringToFront();
            browser.DrawToBitmap(mBitmap, browser.Bounds);
            // Save as file? 
            if (mFileName.Length > 0)
            {
                // Save 
                mBitmap.SavePng100(mFileName);
            }
        }
    }
    public static class BitmapExtensions
    {
        public static void SavePng100(this Bitmap bmp, string filename)
        {
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
            bmp.Save(filename, GetEncoder(ImageFormat.Png), encoderParameters);
        }
        public static void SaveJpg100(this Bitmap bmp, Stream stream)
        {
            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
            bmp.Save(stream, GetEncoder(ImageFormat.Jpeg), encoderParameters);
        }
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders()
            .FirstOrDefault(codec => codec.FormatID == format.Guid);
            // Return 
        }
    }
}