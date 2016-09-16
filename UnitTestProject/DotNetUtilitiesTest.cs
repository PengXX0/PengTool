using System;
using System.Linq;
using System.Text;
using DotNet.Utilities;
using DotNet.Utilities.ConvertChinese;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json;
using ServiceStack.Text;

namespace UnitTestProject
{
    [TestClass]
    public class DotNetUtilitiesTest
    {
        [TestMethod]
        public void RmbTest()
        {
            Assert.IsTrue(214m.ToChinese() == "贰佰壹拾肆元");
        }

        [TestMethod]
        public void ToAnonymousTest()
        {
            var result = "17704961260".ToAnonymous();
            Assert.IsTrue(result == "177****1260");
            result = "00000000000".ToAnonymous();
            Assert.IsTrue(result == "********000");
            result = "12121212121".ToAnonymous();
            Assert.IsTrue(result == "177****1260");
            result = "12341234567".ToAnonymous();
            Assert.IsTrue(result == "177****1260");
            result = "17704961260".ToAnonymous();
            Assert.IsTrue(result == "177****1260");
        }

        [TestMethod]
        public void DateTimeConvert()
        {
            var ddd = Int64.Parse("1463735673");
            var dt = ddd.ToTime();
            Assert.IsNotNull(dt);
        }

        [TestMethod]
        public void testmeth()
        {
            string content = "[{\"Title\":\"SADFSAFSA\",\"Url\":\"SADFSAF\",\"PicUrl\":\"\",\"Description\":\"SDAFSA\"},{\"Title\":\"SADFSAFSA\",\"Url\":\"SADFSAF\",\"PicUrl\":\"\",\"Description\":\"SDAFSA\"},{\"Title\":\"SADFSAFSA\",\"Url\":\"SADFSAF\",\"PicUrl\":\"\",\"Description\":\"SDAFSA\"}]";
            List<Article> list = content.FromJson<List<Article>>();
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void ChineseTest()
        {
            var list = ChineseDictionary.Dictionary;
            var str = JsonConvert.SerializeObject(list);
            Assert.IsNotNull(str);
            //var json = JsonConvert.SerializeObject(ConvertToPinYin.ConvertToPinYins().Select(s => new WordInfo { Chinese = s.Chinese, Pinyin = ConvertToPinYin.PinYinWithTonal(s.Pinyin) }));
            //Assert.IsNotNull(json);
        }
    }

    public class Article
    {
        private static string m_Template;
        /// <summary>
        /// 图文消息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图片链接，支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 图文消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 点击图文消息跳转链接
        /// </summary>
        public string Url { get; set; }
        public string Template
        {
            get
            {
                if (string.IsNullOrEmpty(m_Template))
                {
                    LoadTemplate();
                }
                return m_Template;
            }
        }

        public string GenerateContent()
        {
            return string.Format(this.Template, this.Title, this.Description, this.PicUrl, this.Url);
        }

        private static void LoadTemplate()
        {
            m_Template = @"<item>
                               <Title><![CDATA[{0}]]></Title> 
                               <Description><![CDATA[{1}]]></Description>
                               <PicUrl><![CDATA[{2}]]></PicUrl>
                               <Url><![CDATA[{3}]]></Url>
                            </item>";
        }
    }
}
