using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class WidgetFormMethod : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "WidgetFormMethod"; }
        }

        public string Description
        {
            get { return "Cria os métodos de Form Widgets"; }
        }

        public string Extension
        {
            get { return ".cs"; }
        }

        public string Directory
        {
            get { return null; }
        }

        public string ApplyTemplate(TableModel table, List<TableModel> tables = null, string textToAppend = null)
        {
            _fileName = table.Name.Replace("tb_", "");
            if( string.IsNullOrEmpty(table.Group))
                return "";

            string proxyDeclaration = "";
            string proxyExecution = "";

            List<string> groups = new List<string>();
            foreach (ColumnModel column in table.Columns)
            {
                if (string.IsNullOrEmpty(column.RelatedTable) == false)
                {
                    TableModel relatedTable = base.RelatedTable(column, tables);
                    if (relatedTable != null)
                    {
                        string proxyClass = relatedTable.Group + "Proxy";
                        string proxyName = "_" + relatedTable.Group + "Proxy";
                        string resultName = proxyName + "Result";
                        string tableName = relatedTable.Name.Replace("tb_", "");
                        ColumnModel labelProperty = relatedTable.Columns.Where(c => c.ExtendedProperty == "Label").FirstOrDefault();
                        ColumnModel codeProperty = relatedTable.Columns.Where(c => c.ExtendedProperty == "Codigo").FirstOrDefault();

                        if (labelProperty != null && codeProperty != null)
                        {
                            if (groups.Contains(relatedTable.Group) == false)    
                            {
                                groups.Add(relatedTable.Group);
                                proxyDeclaration += string.Format("\t{0} {1} = new {2}();", proxyClass, proxyName, proxyClass);
                                proxyDeclaration += Environment.NewLine;
                            }
                            proxyExecution += string.Format("\tOutputTransport<List<{0}DTO>> {1} = new {2}.Search{3}(null);", tableName, resultName, proxyName, tableName);
                            proxyExecution += Environment.NewLine;
                            proxyExecution += string.Format("\tif( {0}.Code == 0 && {1}.Data != null )", resultName, resultName);
                            proxyExecution += Environment.NewLine;
                            proxyExecution += "\t{";
                            proxyExecution += Environment.NewLine;
                            proxyExecution += string.Format("\t\tViewBag.{0}List = {1}.Data.Select(r => new SelectListItem", tableName, resultName);
                            proxyExecution += Environment.NewLine;
                            proxyExecution += "\t\t{";
                            proxyExecution += Environment.NewLine;
                            proxyExecution += string.Format("\t\t\tText = r.{0},", labelProperty.ColumnName);
                            proxyExecution += Environment.NewLine;
                            proxyExecution += string.Format("\t\t\tValue = r.{0}.ToString()", codeProperty.ColumnName);
                            proxyExecution += Environment.NewLine;
                            proxyExecution += "\t\t}).ToList();";
                            proxyExecution += Environment.NewLine;
                            proxyExecution += "\t}";
                            proxyExecution += Environment.NewLine;
                            proxyExecution += Environment.NewLine;
                        }
                    }
                }
            }
            StringBuilder ret = new StringBuilder();
            ret.AppendLine("[HttpGet]");
            ret.AppendLine(string.Format("public ActionResult {0}Form()", table.Name.Replace("tb_", "")));
            ret.AppendLine("{");
            ret.Append(proxyDeclaration);
            ret.Append(proxyExecution);
            ret.AppendLine("\treturn View();");
            ret.AppendLine("}");

            return ret.ToString();
        }

        public string FileName
        {
            get { return _fileName; }
        }
    }
}
