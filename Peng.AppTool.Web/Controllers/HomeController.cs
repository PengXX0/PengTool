using DotNet.Utilities;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Peng.AppTool.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Qrcode(string data)
        {
            log.Info(data);
            log.Debug(data);
            log.Error(data);
            log.Fatal(data);
            log.Warn(data);
            return File(QrCodeHelpers.CreateQrCode(data, ImageFormat.Png, 5, ErrorCorrectionLevel.H, QuietZoneModules.Two), "image/png");
        }


        public ActionResult HtmlToJs()
        {
            return View();
        }



        public ActionResult About()
        {
            //ViewBag.Message = "Your app description page.";

            //return View();
            var obj = new { name = "张三", sex = "男", age = 22 };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        protected ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}
