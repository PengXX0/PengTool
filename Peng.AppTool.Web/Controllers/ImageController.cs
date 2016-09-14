using DotNet.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Peng.AppTool.Web.Controllers
{
    public class ImageController : Controller
    {
        //
        // GET: /Image/

        public ActionResult Screenshot(string url)
        {
            if (!Request.IsAjaxRequest()) return View();
            try
            {
                var virtualPath = "~/Content/UploadFiles/";
                var dirpath = Server.MapPath(virtualPath);
                if (!Directory.Exists(dirpath)) { Directory.CreateDirectory(dirpath); }
                var fileName = Guid.NewGuid().ToString("N") + ".png";
                Image img = new WebsiteToImage(url, dirpath + fileName).Generate();
                return Json(new { StatusCode = 200, Data = Url.Content(virtualPath + fileName), Message = "操作成功 ！" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { StatusCode = 200, ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }


        public ActionResult Echarts(string imgString)
        {
            if (!Request.IsAjaxRequest()) return View();
            var pattern = @"^data:image\/+(gif|png|jpg|jpeg+);base64,([a-z,A-Z,0-9,/,+,=,\s]+)$";
            var match = new Regex(pattern, RegexOptions.IgnoreCase).Match(imgString.Replace(' ', '+'));
            if (!match.Success) { throw new Exception("字符串格式不正确！"); }
            var img = Image.FromStream(new MemoryStream(Convert.FromBase64String(match.Groups[2].Value)));
            var dirpath = Server.MapPath("~/Content/UploadFiles/");
            if (!Directory.Exists(dirpath)) { Directory.CreateDirectory(dirpath); }
            var imgExtension = "." + match.Groups[1].Value;
            img.Save(dirpath + Guid.NewGuid().ToString("N") + imgExtension, ImageFormat.Png);
            return Json(new { StatusCode = 200, Message = "操作成功 ！" }, JsonRequestBehavior.DenyGet);
        }

    }
}
