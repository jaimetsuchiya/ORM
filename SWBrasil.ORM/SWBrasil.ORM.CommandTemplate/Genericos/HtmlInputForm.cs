using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class HtmlInputForm : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "HtmlInputForm"; }
        }

        public string Description
        {
            get { return "HtmlInputForm"; }
        }

        public string Extension
        {
            get { return ".html"; }
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public string Directory
        {
            get { return this.NameSpace + ".UI\\Views\\Widgets"; }
        }

        public string ApplyTemplate(TableModel table, List<TableModel> tables = null, string textToAppend = null)
        {
            _fileName = table.Name.Replace("tb_", "")+"Form";

            StringBuilder ret = new StringBuilder();
            StringBuilder js = new StringBuilder();
            foreach (ColumnModel col in table.Columns)
            {
                
                if(col.IsIdentity)
                    ret.AppendLine("@Html.HiddenFor(model => model." + col.ColumnName + ");");

                ret.AppendLine("<div class=\"form-group\">");
                
                if( string.IsNullOrEmpty(col.RelatedTable) == false )
                {
                    ret.AppendLine("    <label for=\"cbo" + col.ColumnName + "\" class=\"col-sm-2 control-label\">" +col.ColumnName + "</label>");
                    ret.AppendLine("    <div class=\"col-sm-4\">");
                    ret.AppendLine("        @Html.DropDownListFor(model => model." + col.ColumnName + ", (IEnumerable<SelectListItem>)ViewBag." + col.RelatedTable.Replace("tb_", "") + "List, new {@class=\"form-control\" })");
                    ret.AppendLine("    </div>");
                }
                else
                {
                    string maxLength = "";
                    string mask = "";
                    switch (col.DbType)
                    {
                        case "DATE":
                            maxLength = "10";
                            break;

                        case "DATETIME":
                            maxLength = "16";
                            break;

                        case "TIME":
                            maxLength = "8";
                            break;

                        case "SMALLINT":
                            maxLength = "5";
                            break;

                        case "MONEY":
                        case "DECIMAL":
                        case "BIGINT":
                        case "INT":
                            maxLength = "10";
                            break;

                        default:
                            if (col.Size.HasValue)
                                maxLength = col.Size.Value.ToString();
                            break;
                    }

                    if( maxLength == "" )
                        maxLength = "maxLength=" + maxLength;

                    ret.AppendLine("    <label for=\"txt" + col.ColumnName + "\" class=\"col-sm-2 control-label\">" +col.ColumnName + "</label>");
                    ret.AppendLine("    <div class=\"col-sm-4\">");
                    ret.AppendLine("        @Html.TextBoxFor(model=>model.building, new {@class=\"form-control\", " + maxLength + "})");
                    ret.AppendLine("    </div>");
                }
                ret.AppendLine("</div>");
            }

            return ret.ToString();
        }
    }
}
