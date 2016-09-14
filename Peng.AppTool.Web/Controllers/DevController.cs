using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peng.AppTool.Web.Controllers
{
    public class DevController : Controller
    {
        //
        // GET: /Dev/

        public ActionResult ApiDoc()
        {
            return Redirect("/Document/index.html");
        }


        public ActionResult Debug()
        {
            return View();
        }

    }
}
