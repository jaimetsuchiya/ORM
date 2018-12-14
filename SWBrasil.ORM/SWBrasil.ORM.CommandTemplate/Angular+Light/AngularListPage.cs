using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class AngularListPage : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "AngularListPage"; }
        }

        public string Description
        {
            get { return "Cria a Pagina de Consulta, baseado no widget Angular"; }
        }

        public string Extension
        {
            get { return ".cshtml"; }
        }

        public string Directory
        {
            get { return _directoryName; }
        }

        public string ApplyTemplate(TableModel table, List<TableModel> tables = null, string textToAppend = null)
        {
            _fileName = "List";

            StringBuilder htmlCode = new StringBuilder();
            if (table.MainDTO)
            {
                string controller = DeCapitalize(table.Alias.Replace("DTO", "") + "Controller");
                string service = controller.Replace("Controller", "SearchWidgetService");
                string baseUrl = "/" + table.Alias.Replace("DTO", "") + "/Details?id=";
                string pk = table.Columns.Where(c => c.IsPK == true).First().DTOName;

                htmlCode.AppendLine("");
                htmlCode.AppendLine("@{");
                htmlCode.AppendLine("\tViewBag.Title = \"" + table.Label + "\";");
                htmlCode.AppendLine("}");
                htmlCode.AppendLine("");
                htmlCode.AppendLine("@{ Html.RenderAction(\"" + table.Alias.Replace("DTO", "") + "SearchWidget\", \"Widgets\");");
                htmlCode.AppendLine("}");
                htmlCode.AppendLine("");
                htmlCode.AppendLine("<div ng-controller=\"" + controller + "\" id=\"" + controller + "\">");
                htmlCode.AppendLine("\t<button type=\"button\" id=\"btnEditar\" class=\"btn btn-info pull-right\" ng-click=\"details();\" style=\"margin:5px;\">Editar " + System.Web.HttpUtility.HtmlEncode(table.Label) + "</button>");
                htmlCode.AppendLine("\t<button type=\"button\" id=\"btnNovo\" class=\"btn btn-success pull-right\" onclick=\"document.location.href = '" + baseUrl + "';\" style=\"margin:5px;\">Novo(a) " + System.Web.HttpUtility.HtmlEncode(table.Label) + "</button>");
                htmlCode.AppendLine("</div>");
                htmlCode.AppendLine("");
                htmlCode.AppendLine("@section scripts {");
                htmlCode.AppendLine("\t<script type=\"text/javascript\" src=\"~/Scripts/Controllers/" + DeCapitalize( table.Alias.Replace("DTO", "") ) + "Controller.js\"></script>");
                htmlCode.AppendLine("}");
            }
            return htmlCode.ToString();
      }


    public string FileName
        {
            get { return _fileName; }
        }
    }
}
