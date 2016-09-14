using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace DotNet.Utilities
{
    /// <summary>
    /// CSV文件转换类
    /// </summary>
    public static class CsvManager
    {
        /// <summary>
        /// 导出报表为Csv
        /// </summary>
        /// <param name="dataTable">DataTable</param>
        /// <param name="strFilePath">物理路径</param>
        /// <param name="tableheader">表头</param>
        /// <param name="columname">字段标题,逗号分隔</param>
        /// <param name="append"></param>
        public static bool ToCsv(this DataTable dataTable, string strFilePath, string tableheader, string columname, bool append = false)
        {
            try
            {
                var strmWriterObj = new StreamWriter(strFilePath, append, Encoding.UTF8);
                strmWriterObj.WriteLine(tableheader);
                strmWriterObj.WriteLine(columname);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var strBufferLine = "";
                    for (var j = 0; j < dataTable.Columns.Count; j++)
                    {
                        if (j > 0)
                            strBufferLine += ",";
                        strBufferLine += dataTable.Rows[i][j].ToString();
                    }
                    strmWriterObj.WriteLine(strBufferLine);
                }
                strmWriterObj.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将Csv读入DataTable
        /// </summary>
        /// <param name="filePath">csv文件路径</param>
        /// <param name="n">表示第n行是字段title,第n+1行是记录开始</param>
        public static DataTable ToDataTable(string filePath, int n)
        {
            var dataTable = new DataTable();
            var reader = new StreamReader(filePath, Encoding.UTF8, false);
            var m = 0;
            while (reader.Peek() > 0)
            {
                m = m + 1;
                var str = reader.ReadLine();
                if (m < n + 1) continue;
                var split = str.Split(',');
                var dr = dataTable.NewRow();
                for (var i = 0; i < split.Length; i++)
                {
                    dr[i] = split[i];
                }
                dataTable.Rows.Add(dr);
            }
            return dataTable;
        }

    }
}
