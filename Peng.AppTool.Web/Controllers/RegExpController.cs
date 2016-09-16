using DotNet.Utilities;
using Peng.AppTool.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Peng.AppTool.Web.Controllers
{
    public class RegExpController : Controller
    {
        //
        // GET: /RegExp/
        [ValidateInput(false)]
        public ActionResult CsharpTest(string pattern, string input)
        {
            if (!Request.IsAjaxRequest()) return View();
            var response = new ResponseModel { Code = HttpStatusCode.OK };
            var match = new Regex(pattern.Replace(' ', '+')).Match(input.Replace(' ', '+'));
            var list = new List<RegExpModels>();
            if (!match.Success)
            {
                response.Data = list;
                return Json(response, JsonRequestBehavior.DenyGet);
            }
            for (var i = 0; i < match.Groups.Count; i++)
            {
                var item = match.Groups[i];
                list.Add(new RegExpModels { Success = item.Success, Index = i, Length = item.Value.Length, Value = item.Value });
            }
            response.Data = list;
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        public ActionResult ScriptTest()
        {
            return View();
        }


        public ActionResult Document()
        {
            return View();
        }
    }
}
