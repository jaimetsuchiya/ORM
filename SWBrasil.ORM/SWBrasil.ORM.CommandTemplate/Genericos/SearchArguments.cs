using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class SearchArguments: CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "SearchArguments"; }
        }

        public string Description
        {
            get { return "Create DTO Search Arguments Structure"; }
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
            _fileName = table.Name.Replace("tb_", "") + "Args";
            _directoryName = this.ProjectName + ".Common\\DTO\\" + table.Group;
            string[] ignoreColumns = new string[] { "criadoEm", "criadoPor", "alteradoEm", "alteradoPor", "versao" };
            string template = @"
public class {0}Args
{
{1}
}";
            StringBuilder columns = new StringBuilder();
            foreach (ColumnModel col in table.Columns.Where(c=>ignoreColumns.Contains(c.ColumnName) ==false).ToList())
            {
                //if (string.IsNullOrEmpty(col.RelatedTable) == false)
                //{
                //    columns.AppendLine("\tpublic " + col.RelatedTable.Replace("tb_", "") + "Args " + col.RelatedTable.Replace("tb_", "") + " { get; set; }");
                //    columns.AppendLine("");
                //}
                //else
                {
                    string dataType = col.DataType;
                    if (dataType != "string")
                        dataType += "?";

                    if (dataType == "DateTime")
                    {
                        columns.AppendLine("\tpublic " + dataType + " " + col.ColumnName + "Ini { get; set; }");
                        columns.AppendLine("\tpublic " + dataType + " " + col.ColumnName + "Fim { get; set; }");
                    }
                    else
                        columns.AppendLine("\tpublic " + dataType + " " + col.ColumnName + " { get; set; }");

                    columns.AppendLine("");
                }
            }

            return template.Replace("{0}", _fileName).Replace("{1}", columns.ToString());
        }
    }
}
