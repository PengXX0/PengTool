using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Peng.AppTool.Web.Models
{
    public class ResponseModel
    {
        /// <summary>
        /// 返回编码
        /// </summary>
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// 返回内容,若为空，则返回空对象
        /// </summary>
        private object _Data;
        public object Data
        {
            get { return _Data ?? new Object(); }
            set { _Data = value; }
        }
    }
}