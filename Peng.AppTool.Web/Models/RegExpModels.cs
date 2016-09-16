using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Peng.AppTool.Web.Models
{
    public class RegExpModels
    {
        /// <summary>
        /// 测试结果
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 结果索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 匹配值的长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 匹配的内容
        /// </summary>
        public string Value { get; set; }
    }
}