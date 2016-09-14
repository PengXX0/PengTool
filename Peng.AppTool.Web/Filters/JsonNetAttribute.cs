using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Peng.AppTool.Web.Filters
{
    public class JsonNetAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is JsonResult == false) { return; }
            filterContext.Result = new JsonNetResult((JsonResult)filterContext.Result);
        }

        private class JsonNetResult : JsonResult
        {
            public JsonNetResult(JsonResult jsonResult)
            {
                ContentEncoding = jsonResult.ContentEncoding;
                ContentType = jsonResult.ContentType;
                Data = jsonResult.Data;
                JsonRequestBehavior = jsonResult.JsonRequestBehavior;
                MaxJsonLength = jsonResult.MaxJsonLength;
                RecursionLimit = jsonResult.RecursionLimit;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                    throw new ArgumentNullException("context");
                if (JsonRequestBehavior == JsonRequestBehavior.DenyGet
                    && String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("GET not allowed! Change JsonRequestBehavior to AllowGet.");
                var response = context.HttpContext.Response;
                response.ContentType = String.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;
                if (ContentEncoding != null)
                    response.ContentEncoding = ContentEncoding;
                if (Data != null)
                    response.Write(JsonConvert.SerializeObject(Data));
            }
        }
    }
}