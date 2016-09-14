using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Peng.AppTool.Web.Models
{
    public class EnumModel
    {

    }

    public enum Country
    {
        请选择 = 0,
        美国 = 1,
        中国 = 2,
        俄罗斯 = 3
    }

    public class EnumExt
    {
        static public List<SelectListItem> ToListItem<T>()
        {
            return (from int s in Enum.GetValues(typeof(T)) select new SelectListItem { Value = s.ToString(), Text = Enum.GetName(typeof(T), s) }).ToList();
        }
    }
}