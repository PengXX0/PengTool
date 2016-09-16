using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace DotNet.Utilities
{
    public class NpoiExcel
    {
        /// <summary>
        /// Excel页(Sheet)模板
        /// </summary>
        public class SheetModel
        {
            /// <summary>
            /// 实例化对象（空实现）
            /// </summary>
            public SheetModel() { }

            /// <summary>
            /// Excel页标签名称
            /// </summary>
            public string SheetName { set; get; }

            /// <summary>
            /// Excel标题
            /// </summary>
            public string ExcelTitle { set; get; }

            /// <summary>
            /// 表格标题 
            /// key:标题名称(行业)
            /// value:数据库中字段类型
            /// </summary>
            public Dictionary<string, string> TableTitle { set; get; }

            /// <summary>
            /// 查询条件
            /// key:查询条件字段
            /// value:查询条件值
            /// </summary>
            public Dictionary<string, string> TableSearch { get; set; }

            /// <summary>
            /// 数据
            /// </summary>
            public DataTable DataTable { set; get; }
        }

        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="path">当前应用程序的物理路径 Server.MapPath("~/");</param>
        /// <param name="sheet">已封装好的Excel Sheet对象</param>
        /// <returns>Excel文件名</returns>
        public string Export(SheetModel sheet, string path)
        {
            var dinfo = new DirectoryInfo(path);
            if (!dinfo.Exists) { dinfo.Create(); }

            //内容行开始的索引
            var rowCount = sheet.TableTitle.Count - 1;
            var wk = new XSSFWorkbook();
            //IWorkbook wk = new HSSFWorkbook();
            //创建一个名称为mySheet的表
            var tb = wk.CreateSheet(sheet.ExcelTitle + "报表");
            //创建一行，此行为第二行
            tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 2, 0, rowCount));
            var row = tb.CreateRow(0);
            var cell = row.CreateCell(0);
            cell.SetCellValue(sheet.ExcelTitle);
            var font = wk.CreateFont();
            font.FontName = "微软雅黑";
            font.Boldweight = (short)FontBoldWeight.Bold;
            //font.Color = HSSFColor.OliveGreen.Black.Index;
            font.FontHeightInPoints = 14;

            var cellstyle = wk.CreateCellStyle();
            cellstyle.Alignment = HorizontalAlignment.Center;
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            cellstyle.SetFont(font);
            //为标题添加样式
            cell.CellStyle = cellstyle;

            //添加统计
            tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 0, rowCount));
            row = tb.CreateRow(3);
            cell = row.CreateCell(0);
            cell.SetCellValue("统计：共" + sheet.DataTable.Rows.Count + "行");

            //添加查询条件
            var rowIndes = 4;
            foreach (var item in sheet.TableSearch)
            {
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndes, rowIndes, 0, rowCount));
                row = tb.CreateRow(rowIndes);
                cell = row.CreateCell(0);
                cell.SetCellValue(item.Key + "：" + item.Value);
                rowIndes++;
            }

            //填充标题
            row = tb.CreateRow(rowIndes); var y = 0;
            foreach (var item in sheet.TableTitle)
            {
                cell = row.CreateCell(y);
                cell.SetCellValue(item.Key);
                //单位格字体加粗
                var fontTitle = wk.CreateFont();
                fontTitle.Boldweight = (short)FontBoldWeight.Bold;
                fontTitle.FontHeightInPoints = 12;
                cellstyle = wk.CreateCellStyle();
                cellstyle.SetFont(fontTitle);
                cell.CellStyle = cellstyle;

                //设定列宽度
                switch (item.Key)
                {
                    case "序号":
                        tb.SetColumnWidth(y, 10 * 200);
                        break;
                    case "URL":
                    case "生产商":
                        tb.SetColumnWidth(y, 25 * 300);
                        break;
                    default:
                        tb.SetColumnWidth(y, 25 * 200);
                        break;
                }
                y++;
            }

            rowIndes++;

            //创建行/单元格，填充数据
            for (var k = 0; k < sheet.DataTable.Rows.Count; k++)
            {
                row = tb.CreateRow(k + rowIndes);
                y = 0;
                foreach (var item in sheet.TableTitle)
                {
                    //在第k行中创建单元格
                    cell = row.CreateCell(y);
                    //循环往第二行的单元格中添加数据
                    cell.SetCellValue(FormatDataByKey(item.Value, sheet.DataTable.Rows[k][item.Value].ToString()));
                    y++;
                }
            }
            //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
            path += "/" + Guid.NewGuid() + ".xlsx";

            var stream = new MemoryStream();
            wk.Write(stream);
            var buf = stream.ToArray();

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, buf.Length);   //向打开的这个xls文件中写入mySheet表并保存。
            }
            return path.Substring(path.IndexOf("UserData", StringComparison.Ordinal));
        }

        /// <summary>
        /// 内容格式化
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string FormatDataByKey(string key, string value)
        {
            String[] array = { "createtime", "itime", "endtime", "connecttime", "offlinetime" };
            if (array.Contains(key.ToLower()))
            {
                return String.IsNullOrWhiteSpace(value) ? "" :
                  DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            return value;
        }

        #endregion

        #region 导入Excel
        /// <summary>
        /// 读取2003/2007Excel文件内容返回DataTable
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public DataTable ExcelToDataTable(string path, string fileName)
        {
            var dt = new DataTable();
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // var workbook = new HSSFWorkbook(fs);//只能识别 2003
                // var workbook = new XSSFWorkbook(fs);//只能识别 2007
                var workbook = WorkbookFactory.Create(fs); //自动识别2003、2007
                var sheet1 = workbook.GetSheetAt(0);
                var row1 = sheet1.GetRow(0);  //获取列头
                int cellCount = row1.LastCellNum; //列数
                dt.Columns.Add(new DataColumn("UId"));
                //将列头添加到DataTable中
                for (int i = row1.FirstCellNum; i < cellCount; i++)
                {
                    var dc = new DataColumn(row1.GetCell(i).StringCellValue);
                    dt.Columns.Add(dc);
                }
                var rowCount = sheet1.LastRowNum; //总行数
                //循环将数据添加到DataTable中
                for (var i = (sheet1.FirstRowNum + 1); i <= rowCount; i++)
                {
                    var row = sheet1.GetRow(i);
                    var dr = dt.NewRow();
                    dr[0] = fileName;
                    for (int k = row.FirstCellNum; k < cellCount; k++)
                    {
                        if (row.GetCell(k) != null)
                            dr[k + 1] = row.GetCell(k).ToString();
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// 读取2003/2007Excel文件内容返回DataTable，只读数据，不添加任何列
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable ExcelToDataTable(string path)
        {
            var dt = new DataTable();
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var workbook = WorkbookFactory.Create(fs); //自动识别2003、2007
                var sheet1 = workbook.GetSheetAt(0);
                var row1 = sheet1.GetRow(0);  //获取列头
                int cellCount = row1.LastCellNum; //列数
                //将列头添加到DataTable中
                for (int i = row1.FirstCellNum; i < cellCount; i++)
                { dt.Columns.Add(new DataColumn(row1.GetCell(i).StringCellValue)); }
                var rowCount = sheet1.LastRowNum; //总行数
                //循环将数据添加到DataTable中
                for (var i = (sheet1.FirstRowNum + 1); i <= rowCount; i++)
                {
                    var row = sheet1.GetRow(i);
                    var dr = dt.NewRow();
                    for (int k = row.FirstCellNum; k < cellCount; k++)
                    {
                        if (row.GetCell(k) != null)
                            dr[k] = row.GetCell(k).ToString();
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        #endregion
    }
}
