using System.Web;
using System.Web.Mvc;
using Peng.AppTool.Web.Filters;

namespace Peng.AppTool.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new JsonNetAttribute());
        }
    }
}