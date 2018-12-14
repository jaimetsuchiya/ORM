using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class SimpleModel: CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "SimpleModel"; }
        }

        public string Description
        {
            get { return "Create a Simple Model for DTO"; }
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
            get { return null; }
        }

        public string ApplyTemplate(TableModel table, List<TableModel> tables = null, string textToAppend = null)
        {
            _fileName = table.Name;
            string template = @"
public class {0}DTO
{
{1}
}";
            StringBuilder columns = new StringBuilder();
            foreach (ColumnModel col in table.Columns)
            {
                if (col.Required)
                    columns.AppendLine("\t[Required]");

                columns.AppendLine("\tpublic " + col.DataType + " " + col.ColumnName + " { get; set; }");
                columns.AppendLine("");
                columns.AppendLine("");
            }

            return template.Replace("{0}", table.Name).Replace("{1}", columns.ToString());
        }
    }
}
