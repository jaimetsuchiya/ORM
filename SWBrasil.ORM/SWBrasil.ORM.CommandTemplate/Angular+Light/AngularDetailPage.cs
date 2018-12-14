using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class AngularDetailPage : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "AngularDetailPage"; }
        }

        public string Description
        {
            get { return "Cria a Pagina de Detalhes, baseado no Angular"; }
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
            _fileName = "Details";

            StringBuilder htmlCode = new StringBuilder();
            if (table.MainDTO)
            {
                string controller = DeCapitalize(table.Alias.Replace("DTO", "") + "Controller");
                string service = controller.Replace("Controller", "SearchWidgetService");
                string baseUrl = "/" + table.Alias.Replace("DTO","") + "/Details?id=";
                string pk = table.Columns.Where(c => c.IsPK == true).First().DTOName;
                string form = "frm" + table.Alias.Replace("DTO", "");

                htmlCode.AppendLine("");
                htmlCode.AppendLine("@{");
                htmlCode.AppendLine("\tViewBag.Title = \"" + System.Web.HttpUtility.HtmlEncode(table.Label) + "\";");
                htmlCode.AppendLine("}");
                htmlCode.AppendLine("");
                htmlCode.AppendLine("");
                htmlCode.AppendLine("<div ng-controller=\"" + controller + "\"  id=\"" + controller + "\">");
                htmlCode.AppendLine("\t<div class=\"row\">");
                htmlCode.AppendLine("\t\t<div class=\"col-xs-12 col-md-6 col-lg-4\">");
                htmlCode.AppendLine("\t\t\t<div class=\"box box-info\">");
                htmlCode.AppendLine("\t\t\t\t<div class=\"box-header with-border\">");
                htmlCode.AppendLine("\t\t\t\t\t<h3 class=\"box-title\">" + System.Web.HttpUtility.HtmlEncode(table.Label) + "</h3>");
                htmlCode.AppendLine("\t\t\t\t\t<div class=\"box-tools pull-right\">");
                htmlCode.AppendLine("\t\t\t\t\t\t<button type=\"button\" class=\"btn btn-box-tool\" data-widget=\"collapse\"><i class=\"fa fa-minus\"></i></button>");
                htmlCode.AppendLine("\t\t\t\t\t</div>");
                htmlCode.AppendLine("\t\t\t\t</div>");
                htmlCode.AppendLine("\t\t\t\t<div class=\"box-body\">");
                htmlCode.AppendLine("\t\t\t\t\t<form class=\"form-horizontal\" name=\"" + form + "\">");
                foreach(ColumnModel col in table.Columns.Where(c=>c.IgnoreOnDTO == false).OrderBy(c=>c.Position).ToList())
                {
                    string required = (col.Required && col.IsIdentity == false ? " ng-required=\"true\" required ": "");
                    string ngModel = string.Format(" ng-model=\"{0}\" ", "app.Data." + col.DTOName);
                    string fieldName = "txt" + Capitalize(col.DTOName);

                    htmlCode.AppendLine("\t\t\t\t\t\t<div class=\"col-xs-12 col-lg-4 col-md-6\">");
                    htmlCode.AppendLine("\t\t\t\t\t\t\t<div class=\"form-group\">");
                    htmlCode.AppendLine("\t\t\t\t\t\t\t\t<label for=\"txtId\">" + System.Web.HttpUtility.HtmlEncode(col.Label) + "</label>");

                    if (col.UseAsRelatedObject && string.IsNullOrEmpty(col.RelatedTable) == false )
                    {
                        var relatedTable = tables.Where(t => t.Name == col.RelatedTable).FirstOrDefault();
                        
                        if ( col.SelectionType == enumSelectionType.ComboBox )
                        {
                            var relatedPKColumn = relatedTable.Columns.Where(c => c.IsPK).FirstOrDefault();
                            var relatedLabelColumn = relatedTable.Columns.Where(c => c.UseAsLabelOnComboBox).FirstOrDefault();
                            ngModel = string.Format(" ng-model=\"{0}\" ", "app.Data." + relatedTable.Alias.Replace("DTO", "") + "." + relatedPKColumn.DTOName);
                            fieldName = "cbo" + Capitalize(col.DTOName);

                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<select class=\"form-control\"id=\"" + fieldName + "\"  name=\"" + fieldName + "\"  " + required + ngModel + ">");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t<option value=\"\">-- Selecione --</option>");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t@if( ViewBag.LST_" + relatedTable.Alias.Replace("DTO", "") + " != null && ViewBag.LST_" + relatedTable.Alias.Replace("DTO", "") + ".success)");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t{");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t\tfor( var i=0; i < ViewBag.LST_" + relatedTable.Alias.Replace("DTO", "") + ".Data.Count; i++)");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t\t{");
                            if(relatedPKColumn != null && relatedLabelColumn != null)
                                htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t\t\t<option value=\"@(ViewBag.LST_" + relatedTable.Alias.Replace("DTO", "") + ".Data[i]." + relatedPKColumn.DTOName + ")\" >@(ViewBag.LST_" + relatedTable.Alias.Replace("DTO", "") + ".Data[i]." + relatedLabelColumn.DTOName + ")</option>");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t\t}");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t}");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t</select>");
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t<input type=\"text\" class=\"form-control\" id=\"" + fieldName + "\" name=\"" + fieldName + "\" placeholder=\"" + System.Web.HttpUtility.HtmlEncode( col.Label ) + "\" " + required + ngModel + "/>");
                    }
                    if (col.Required && col.IsIdentity == false)
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t<p ng-show=\"" + form + "." + fieldName + ".$invalid\" class=\"label-danger\">" + System.Web.HttpUtility.HtmlEncode( col.Label + " não " ) + "informado</p>");
                    htmlCode.AppendLine("\t\t\t\t\t\t\t</div>");
                    htmlCode.AppendLine("\t\t\t\t\t\t</div>");
                }

                htmlCode.AppendLine("\t\t\t\t\t</form>");
                htmlCode.AppendLine("\t\t\t\t</div>");
                htmlCode.AppendLine("\t\t\t\t<div class=\"box-footer\">");
                htmlCode.AppendLine("\t\t\t\t\t<button type=\"button\" id=\"btnSalvar\" class=\"btn btn-success pull-right\" ng-disabled=\"" + form + ".$invalid\" ng-click=\"save(app.Data);\" style=\"margin:5px;\">Salvar</button>");
                htmlCode.AppendLine("\t\t\t\t\t<button type=\"button\" id=\"btnVoltar\" class=\"btn btn-info pull-right\" onclick=\"document.location.href='/" + table.Alias.Replace("DTO", "") + "/List';\" style=\"margin:5px;\">Voltar</button>");
                htmlCode.AppendLine("\t\t\t\t</div>");
                htmlCode.AppendLine("\t\t\t</div>");
                htmlCode.AppendLine("\t\t</div>");
                htmlCode.AppendLine("\t</div>");
                htmlCode.AppendLine("</div>");
                htmlCode.AppendLine("");

                htmlCode.AppendLine("@section scripts {");
                htmlCode.AppendLine("\t<script type=\"text/javascript\" src=\"~/Scripts/Controllers/" + DeCapitalize(table.Alias.Replace("DTO", "")) + "Controller.js\"></script>");
                htmlCode.AppendLine("\t<script type=\"text/javascript\">");
                htmlCode.AppendLine("\t\t$(document).ready(function() {");
                htmlCode.AppendLine("\t\t\tangular.element(document.getElementById('" + controller + "')).scope().load('@ViewBag.Id');");
                htmlCode.AppendLine("\t\t});");
                htmlCode.AppendLine("\t</script>");
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
