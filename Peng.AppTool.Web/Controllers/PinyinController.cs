using DotNet.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Utilities.ConvertChinese;

namespace Peng.AppTool.Web.Controllers
{
    public class PinyinController : Controller
    {
        //
        // GET: /Pinyin/

        public ActionResult Pinyin(string content)
        {
            if (!Request.IsAjaxRequest()) return View();
            var pinyin = ChineseDictionary.ToPinyin(content); //EcanConvertToCh.ConvertCh(content);
            return Json(pinyin, JsonRequestBehavior.AllowGet);
        }
    }
}
