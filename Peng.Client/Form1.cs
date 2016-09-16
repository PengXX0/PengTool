using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Peng.Client.Properties;

namespace Peng.Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var html = String.Format(Resources.HtmlString, 600, 400, Resources.script);
            browser.Document.Write(html);
            browser.Document.InvokeScript("run", new Object[] { GetJson() });
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            buttonLoad.Enabled = false;
            var imgString = browser.Document.GetElementById("i").GetAttribute("value");
            var filePath = AppDomain.CurrentDomain.BaseDirectory + "../../ImageFiles/";
            Base64ToImage(imgString, filePath);
        }

        public static string GetJson()
        {
            return @"{
        tooltip: {
            trigger: 'axis',
            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        legend: {
            data: ['直接访问', '邮件营销', '联盟广告', '视频广告', '搜索引擎', '百度', '谷歌', '必应', '其他']
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: [
            {
                type: 'category',
                data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
            }
        ],
        yAxis: [
            {
                type: 'value'
            }
        ],
        series: [
            {
                name: '直接访问',
                type: 'bar',
                data: [320, 332, 301, 334, 390, 330, 320]
            },
            {
                name: '邮件营销',
                type: 'bar',
                stack: '广告',
                data: [120, 132, 101, 134, 90, 230, 210]
            },
            {
                name: '联盟广告',
                type: 'bar',
                stack: '广告',
                data: [220, 182, 191, 234, 290, 330, 310]
            },
            {
                name: '视频广告',
                type: 'bar',
                stack: '广告',
                data: [150, 232, 201, 154, 190, 330, 410]
            },
            {
                name: '搜索引擎',
                type: 'bar',
                data: [862, 1018, 964, 1026, 1679, 1600, 1570],
                markLine: {
                    lineStyle: {
                        normal: {
                            type: 'dashed'
                        }
                    },
                    data: [
                        [{ type: 'min' }, { type: 'max' }]
                    ]
                }
            },
            {
                name: '百度',
                type: 'bar',
                barWidth: 5,
                stack: '搜索引擎',
                data: [620, 732, 701, 734, 1090, 1130, 1120]
            },
            {
                name: '谷歌',
                type: 'bar',
                stack: '搜索引擎',
                data: [120, 132, 101, 134, 290, 230, 220]
            },
            {
                name: '必应',
                type: 'bar',
                stack: '搜索引擎',
                data: [60, 72, 71, 74, 190, 130, 110]
            },
            {
                name: '其他',
                type: 'bar',
                stack: '搜索引擎',
                data: [62, 82, 91, 84, 109, 110, 120]
            }
        ]
    }";
        }

        public void Base64ToImage(string imgBase64, string filePath)
        {
            var pattern = @"^data:image\/+(gif|png|jpg|jpeg+);base64,([a-zA-Z0-9,/,+,=,\s]+)$";
            var match = new Regex(pattern, RegexOptions.IgnoreCase).Match(imgBase64);
            if (!match.Success) { throw new Exception("字符串格式不正确！"); }
            var img = Image.FromStream(new MemoryStream(Convert.FromBase64String(match.Groups[2].Value.Replace(" ", "+"))));
            var dirpath = filePath.Substring(0, filePath.LastIndexOf('/'));
            if (!Directory.Exists(dirpath)) { Directory.CreateDirectory(dirpath); }
            img.Save(dirpath + "/" + Guid.NewGuid().ToString("N") + "." + match.Groups[1]);
            MessageBox.Show(" 操作成功！", " 提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            buttonLoad.Enabled = true;
        }
    }
}
