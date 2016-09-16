using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Peng.AppTool.Web.Filters
{
    public class AuthenticationAttribute : AuthorizeAttribute
    {
        static Dictionary<string, object> NvcToDictionary(NameValueCollection nvc, bool handleMultipleValuesPerKey)
        {
            var result = new Dictionary<string, object>();
            foreach (string key in nvc.Keys)
            {
                if (handleMultipleValuesPerKey)
                {
                    string[] values = nvc.GetValues(key);
                    if (values != null && values.Length == 1)
                    {
                        result.Add(key, values[0]);
                    }
                    else
                    {
                        result.Add(key, values);
                    }
                }
                else
                {
                    result.Add(key, nvc[key]);
                }
            }
            return result;
        }


        public static bool CheckSign(NameValueCollection nameValue)
        {
            if (nameValue.AllKeys.Contains("sign"))
            {
                var sign = nameValue["sign"];
                nameValue.Remove("sign");
                var signStr = nameValue.AllKeys.OrderBy(q => q).Aggregate("", (c, q) => c + nameValue[q] + "niuduz");
                //TODO  加密 signStr      DES3、Hash 、MD5
                return sign == signStr;
            }
            return false;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = HttpContext.Current.Request;
            NameValueCollection data = null;
            switch (request.HttpMethod.ToUpper())
            {
                case "POST":
                    data = request.Form;
                    break;
                case "GET":
                    data = request.QueryString;
                    break;
            }
            if (!CheckSign(data))
            {
                string json = new JavaScriptSerializer().Serialize(data);
                //将json、ip等相关信息记录到日志

                var jsonResult = new JsonResult();
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                jsonResult.Data = new { };//返回失败提示数据
                filterContext.Result = jsonResult;
            }


            if (filterContext == null)
                throw new ArgumentNullException("filterContent");

            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();
            if (!HttpContext.Current.User.Identity.IsAuthenticated /*|| HttpContext.Current.Session["CurrentUser"] == null*/)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }


            //记录用户操作
            //string module = "/" + controllerName + "/" + actionName;
            //string what = HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString.ToString());
            //SystemSettingCore.AddLog(module, CommonHelper.QueryStringToJson(what));
        }
    }
}