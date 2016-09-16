using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Text;

namespace Pengxsoft.DbToModel
{
    public static class BusinessUtilities
    {
        public static List<DbModel> GetDbStructures()
        {
            #region SQL语句
            const string sql = @"
SELECT 
    [Id]                   = ROW_NUMBER() OVER(ORDER BY A.ID,A.COLORDER),
    [TableName]            = D.NAME,
    [TableDescription]     = CASE WHEN A.COLORDER=1 THEN ISNULL(F.VALUE,'') ELSE '' END,
    [FieldIndex]           = A.COLORDER,
    [FieldName]            = A.NAME,
    [Identification]       = CASE WHEN COLUMNPROPERTY( A.ID,A.NAME,'ISIDENTITY')=1 THEN 1 ELSE 0 END,
    [Primarykey]           = CASE WHEN EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE='PK' AND PARENT_OBJ=A.ID AND NAME IN (
                           SELECT NAME FROM SYSINDEXES WHERE INDID IN( SELECT INDID FROM SYSINDEXKEYS WHERE ID = A.ID AND COLID=A.COLID))) THEN 1 ELSE 0 END,
    [FieldType]            = B.NAME,
    [OccupancyBytes]       = A.LENGTH,
    [Length]			   = COLUMNPROPERTY(A.ID,A.NAME,'PRECISION'),
    [DecimalDigits]		   = ISNULL(COLUMNPROPERTY(A.ID,A.NAME,'SCALE'),0),
    [IsNull]			   = CASE WHEN A.ISNULLABLE=1 THEN 1 ELSE 0 END,
    [Default]			   = ISNULL(E.TEXT,''),
    [FieldDescription]     = ISNULL(G.[VALUE],'')
FROM 
    SYSCOLUMNS A
LEFT JOIN 
    SYSTYPES B 
ON 
    A.XUSERTYPE=B.XUSERTYPE
INNER JOIN 
    SYSOBJECTS D 
ON 
    A.ID=D.ID  AND D.XTYPE='U' AND  D.NAME<>'DTPROPERTIES'
LEFT JOIN 
    SYSCOMMENTS E 
ON 
    A.CDEFAULT=E.ID
LEFT JOIN 
SYS.EXTENDED_PROPERTIES   G 
ON 
    A.ID=G.MAJOR_ID AND A.COLID=G.MINOR_ID  
LEFT JOIN
SYS.EXTENDED_PROPERTIES F
ON 
    D.ID=F.MAJOR_ID AND F.MINOR_ID=0

ORDER BY 
    A.ID,A.COLORDER";
            #endregion
            var reader = SqlHelper.ExecuteReader(SqlHelper.GetConnSting(), CommandType.Text, sql);
            var list = new List<DbModel>();
            while (reader.Read())
            {
                var model = new DbModel();
                model.Id = Convert.ToInt32(reader["Id"]);
                model.TableName = reader["TableName"].ToString();
                model.TableDescription = reader["TableDescription"].ToString();
                model.FieldIndex = Convert.ToInt32(reader["FieldIndex"]);
                model.FieldName = reader["FieldName"].ToString();
                model.Identification = Convert.ToBoolean(reader["Identification"]);
                model.Primarykey = Convert.ToBoolean(reader["Primarykey"]);
                model.FieldType = reader["FieldType"].ToString();
                model.OccupancyBytes = Convert.ToInt32(reader["OccupancyBytes"]);
                model.Length = Convert.ToInt32(reader["Length"]);
                model.DecimalDigits = Convert.ToInt32(reader["DecimalDigits"]);
                model.IsNull = Convert.ToBoolean(reader["IsNull"]);
                model.Default = reader["Default"].ToString();
                model.FieldDescription = reader["FieldDescription"].ToString();
                list.Add(model);
            }
            return list;
        }

        public static void GenerateTemplate(String content, String tableName = "")
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "/../../Models/";
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            var file = path + (String.IsNullOrEmpty(tableName) ? "Models.cs" : tableName + ".cs");
            using (var sw = new StreamWriter(file, false, Encoding.Default))
            {
                sw.WriteLine(content);
            }
        }

        public static String FrameTemplate(String modelsString)
        {
            var builder = new StringBuilder()
            .Append("using System;").Append(Environment.NewLine)
            .Append("using System.ComponentModel;").Append(Environment.NewLine)
            .Append("using System.ComponentModel.DataAnnotations;").Append(Environment.NewLine)
            .Append("using System.ComponentModel.DataAnnotations.Schema;").Append(Environment.NewLine)
            .Append(Environment.NewLine)
            .Append("namespace Peng.DbToModel.Models").Append(Environment.NewLine)
            .Append("{").Append(Environment.NewLine)
            .AppendFormat("{0}", modelsString)
            .Append("}").Append(Environment.NewLine);
            return builder.ToString();
        }

        public static String ModelTemplate(List<DbModel> list)
        {
            var fielsTemplate = "";
            var builder = new StringBuilder()
            .Append("    /// <summary>").Append(Environment.NewLine)
            .Append("    /// {0}").Append(Environment.NewLine)
            .Append("    /// </summary>").Append(Environment.NewLine)
            .Append("    [Table(\"{1}\")]").Append(Environment.NewLine)
            .Append("    public class {1}").Append(Environment.NewLine)
            .Append("    {{").Append(Environment.NewLine)
            .Append("{2}")
            .Append("    }}").Append(Environment.NewLine);
            foreach (var item in list)
            {
                var fiel = new StringBuilder()
                .Append("        /// <summary>").Append(Environment.NewLine)
                .Append("        /// {0}").Append(Environment.NewLine)
                .Append("        /// </summary>").Append(Environment.NewLine)
                .Append("        [DisplayName(\"{1}\")]").Append(Environment.NewLine);
                if (item.Primarykey)
                    fiel.Append("        [Key]").Append(Environment.NewLine);
                if (!item.IsNull)
                    fiel.Append("        [Required(ErrorMessage = \"{1} is required\")]").Append(Environment.NewLine);
                if (item.Identification)
                    fiel.Append("        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]").Append(Environment.NewLine);
                fiel
                .Append("        public {2} {1} {{ get; set; }}").Append(Environment.NewLine);
                fielsTemplate += String.Format(fiel.ToString(), item.FieldDescription.Replace("\r\n", ""), item.FieldName, GetPropertyType(item.FieldType));
            }
            return String.Format(builder.ToString(), list[0].TableDescription.Replace("\r\n", ""), list[0].TableName, fielsTemplate);
        }

        public static String GetPropertyType(String sqlType)
        {
            String sysType = "String";
            switch (sqlType)
            {
                case "nvarchar":
                case "varchar":
                    sysType = "String";
                    break;
                case "bigint":
                    sysType = "long";
                    break;
                case "smallint":
                    sysType = "short";
                    break;
                case "int":
                    sysType = "int";
                    break;
                case "uniqueidentifier":
                    sysType = "Guid";
                    break;
                case "smalldatetime":
                case "datetime":
                case "datetime2":
                case "date":
                case "time":
                    sysType = "DateTime";
                    break;
                case "datetimeoffset":
                    sysType = "DateTimeOffset";
                    break;
                case "float":
                    sysType = "double";
                    break;
                case "real":
                    sysType = "float";
                    break;
                case "numeric":
                case "smallmoney":
                case "decimal":
                case "money":
                    sysType = "decimal";
                    break;
                case "tinyint":
                    sysType = "byte";
                    break;
                case "bit":
                    sysType = "bool";
                    break;
                case "image":
                case "binary":
                case "varbinary":
                case "timestamp":
                    sysType = "byte[]";
                    break;
                case "geography":
                    sysType = "Microsoft.SqlServer.Types.SqlGeography";
                    break;
                case "geometry":
                    sysType = "Microsoft.SqlServer.Types.SqlGeometry";
                    break;
            }
            return sysType;
        }
    }

    /// <summary>
    /// 数据库表结构
    /// </summary>
    public class DbModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表说明
        /// </summary>
        public string TableDescription { get; set; }
        /// <summary>
        /// 字段序号
        /// </summary>
        public int FieldIndex { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 是否为标识列
        /// </summary>
        public bool Identification { get; set; }
        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool Primarykey { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }
        /// <summary>
        /// 占用字节数
        /// </summary>
        public int OccupancyBytes { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 小数位数
        /// </summary>
        public int DecimalDigits { get; set; }
        /// <summary>
        /// 是否允许空
        /// </summary>
        public bool IsNull { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string Default { get; set; }
        /// <summary>
        /// 字段说明
        /// </summary>
        public string FieldDescription { get; set; }
    }
}
