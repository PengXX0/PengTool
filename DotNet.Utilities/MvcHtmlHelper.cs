using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace DotNet.Utilities
{
    public static class MvcHtmlHelper
    {
        /// <summary>
        /// DropDownList拓展
        /// </summary>
        /// <typeparam name="enumType">枚举</typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="name">字段名称</param>
        /// <param name="htmlAttributes">html属性</param>
        /// <returns></returns>
        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, Type enumType, Object htmlAttributes = null)
        {
            var list = (from int s in Enum.GetValues(enumType) select new SelectListItem { Value = s.ToString(), Text = Enum.GetName(enumType, s) }).ToList();
            var selectTag = new TagBuilder("select");
            selectTag.GenerateId(name);
            selectTag.MergeAttribute("name", name);
            selectTag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            foreach (var item in list)
            {
                var optionTag = new TagBuilder("option");
                optionTag.SetInnerText(item.Text);
                if (item.Selected) { optionTag.MergeAttribute("selected", "selected"); }
                optionTag.MergeAttribute("value", item.Value);
                selectTag.InnerHtml += optionTag.ToString();
            }
            return MvcHtmlString.Create(selectTag.ToString(TagRenderMode.Normal));
        }
    }


}
