using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class EcmsNapierBaseModel : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "EcmsNapierBaseModel"; }
        }

        public string Description
        {
            get { return "Cria Model´s de Persistência no Padrão Napier Ecms"; }
        }
        public string Extension
        {
            get { return ".cs"; }
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public string Directory
        {
            get { return _directoryName; }
        }

        public string ApplyTemplate(TableModel table, List<TableModel> tables = null, string textToAppend = null)
        {
            _messages.Add(new ProjectConsoleMessages() { erro = false, data = DateTime.Now, mensagem = string.Format("{0} - Processando Tabela [{1}]", this.CommandID, table.Name) });
            _fileName = table.ModelName.Replace("Model", "");

            StringBuilder classCode = new StringBuilder();
            classCode.AppendLine("using System;");
            classCode.AppendLine("using Napier.Data;");
            classCode.AppendLine("using Napier.Mapper;");
            classCode.AppendLine("");
            classCode.AppendLine("namespace WebStore.Sites.Data");
            classCode.AppendLine("{");
            classCode.AppendLine("");
            classCode.AppendLine("\t[MapperClass(Storable = \"" + table.Name + "\")]");
            classCode.AppendLine("\tpublic partial class " + table.ModelName.Replace("Model", "") + " : DataAccessLibrary");
            classCode.AppendLine("\t{");
            foreach (ColumnModel col in table.Columns)
            {
                string isPK = col.IsPK ? ", IsKey = true" : "";
                string isIdentity = col.IsIdentity ? ", IsIdentity = true" : "";

                //if (col.IsPK)
                //    col.DataType = col.DataType;

                classCode.AppendLine(string.Format("\t\t[MapperAttribute(Name = \"{0}\" {1} {2})]", col.ColumnName, isPK, isIdentity));
                classCode.AppendLine(string.Format("\t\tpublic {0} {1}", col.DataType, col.ColumnName) + " { get; set; }");
                classCode.AppendLine("");
            }
            classCode.AppendLine("");
            classCode.AppendLine("\t}");
            classCode.AppendLine("}");

            return classCode.ToString();
        }


        //private void buildSearchParameters(TableModel table, List<TableModel> tables, ref string parameters, ref int deep)
        //{
        //    deep++;
        //    foreach (ColumnModel col in table.Columns)
        //    {
        //        if (string.IsNullOrEmpty(col.RelatedTable) == false)
        //        {
        //            TableModel rTable = base.RelatedTable(col, tables);
        //            if (deep > 3)
        //                return;
        //            buildSearchParameters(rTable, tables, ref parameters, ref deep);
        //        }
        //        else
        //        {
        //            switch (col.DataType)
        //            {
        //                case "DateTime":
        //                    parameters += "\t\t\tif(args." + col.ColumnName + "Ini.HasValue)" + Environment.NewLine;
        //                    parameters += "\t\t\t\tcmd.Parameters.Add(new SqlParameter(\"@" + table.Name.Replace("tb_", "") + "_" + col.ColumnName + "Ini\", args." + col.ColumnName + "Ini));" + Environment.NewLine;

        //                    parameters += "\t\t\tif(args." + col.ColumnName + "Fim.HasValue)" + Environment.NewLine;
        //                    parameters += "\t\t\t\tcmd.Parameters.Add(new SqlParameter(\"@" + table.Name.Replace("tb_", "") + "_" + col.ColumnName + "Fim\", args." + col.ColumnName + "Fim));" + Environment.NewLine;
        //                    break;

        //                case "string":
        //                    parameters += "\t\t\tif(string.isNullOrEmpty(args." + col.ColumnName + ") == false)" + Environment.NewLine;
        //                    parameters += "\t\t\t\tcmd.Parameters.Add(new SqlParameter(\"@" + table.Name.Replace("tb_", "") + "_" + col.ColumnName + "\", args." + col.ColumnName + "));" + Environment.NewLine;
        //                    break;

        //                default:
        //                    parameters += "\t\t\tif(args." + col.ColumnName + ".HasValue)" + Environment.NewLine;
        //                    parameters += "\t\t\t\tcmd.Parameters.Add(new SqlParameter(\"@" + table.Name.Replace("tb_", "") + "_" + col.ColumnName + "\", args." + col.ColumnName + "));" + Environment.NewLine;
        //                    break;
        //            }
        //        }
        //    }
        //}
    }
}
