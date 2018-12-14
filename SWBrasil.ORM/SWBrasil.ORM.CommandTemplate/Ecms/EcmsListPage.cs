using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class EcmsListPage : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "EcmsListPage"; }
        }

        public string Description
        {
            get { return "Cria a Pagina de Consulta, baseado no ECMS"; }
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
            _fileName = "Consulta";

            StringBuilder htmlCode = new StringBuilder();
            if (table.MainDTO)
            {
                #region Header

                htmlCode.AppendLine("");
                htmlCode.AppendLine("@{");
                htmlCode.AppendLine("\tViewBag.Title = \"" + System.Web.HttpUtility.HtmlEncode( table.Label ) + " - Consulta\";");
                htmlCode.AppendLine("}");
                htmlCode.AppendLine("");
                htmlCode.AppendLine("@{ Webstore.Ecms.Models.SelecionaMigracaoViewModel _selecionamigracao = (Webstore.Ecms.Models.SelecionaMigracaoViewModel)ViewBag.migracao; }");
                htmlCode.AppendLine("@model List<Webstore.Ecms.Models." + table.Alias.Replace("DTO", "") + "ViewModel>");
                htmlCode.AppendLine("");
                htmlCode.AppendLine("@section topnavbar{");
                htmlCode.AppendLine("\t<div id=\"head-nav\" class=\"navbar navbar-default\">");
                htmlCode.AppendLine("\t\t<div class=\"container-fluid\">");
                htmlCode.AppendLine("\t\t\t<div class=\"navbar-collapse\">");
                htmlCode.AppendLine("\t\t\t\t@Html.Partial(\"_selecionasite\", _selecionamigracao)");
                htmlCode.AppendLine("\t\t\t\t@Html.Partial(\"_usuariologado\")");
                htmlCode.AppendLine("\t\t\t</div>");
                htmlCode.AppendLine("\t\t</div>");
                htmlCode.AppendLine("\t</div>");
                htmlCode.AppendLine("}");
                htmlCode.AppendLine("");
                htmlCode.AppendLine("");

                #endregion Header

                htmlCode.AppendLine("<div class=\"cl-mcont\" data-page=\"" + DeCapitalize(table.Alias.Replace("DTO", "")) + "List\">");

                #region BreadCrumb Row 

                htmlCode.AppendLine("\t<div class=\"row\">");
                htmlCode.AppendLine("\t\t<div class=\"page-head col-sm-7\">");
                htmlCode.AppendLine("\t\t\t<h3 class=\"hthin\">Você está em: <strong>" + System.Web.HttpUtility.HtmlEncode(table.Label) + " - Consulta</strong></h3>");
                htmlCode.AppendLine("\t\t\t<ol class=\"breadcrumb\">");
                htmlCode.AppendLine("\t\t\t\t<li><a href=\"#\">Ecms</a></li>");
                htmlCode.AppendLine("\t\t\t\t<li>" + System.Web.HttpUtility.HtmlEncode(table.Label) + "</li>");
                htmlCode.AppendLine("\t\t\t\t<li>Consulta</li>");
                htmlCode.AppendLine("\t\t\t</ol>");
                htmlCode.AppendLine("\t\t</div>");
                htmlCode.AppendLine("\t\t<div class=\"page-opts col-sm-3 pull-right\"></div>");
                htmlCode.AppendLine("\t</div>");

                #endregion BreadCrumb Row 

                #region Filter Criteria
                htmlCode.AppendLine("\t<div class=\"row\">");
                htmlCode.AppendLine("\t\t<div class=\"col-md-12\">");
                htmlCode.AppendLine("\t\t\t<div class=\"block-flat\">");
                htmlCode.AppendLine("\t\t\t\t<div class=\"header\">");
                htmlCode.AppendLine("\t\t\t\t\t<div class=\"actions\">");
                htmlCode.AppendLine("\t\t\t\t\t\t<a class=\"minimize\" href=\"#\"><i class=\"fa fa-chevron-down\"></i></a>");
                htmlCode.AppendLine("\t\t\t\t\t</div>");
                htmlCode.AppendLine("\t\t\t\t\t<h3>Pesquisar</h3>");
                htmlCode.AppendLine("\t\t\t\t</div>");
                htmlCode.AppendLine("\t\t\t\t<div class=\"content\">");
                htmlCode.AppendLine("\t\t\t\t\t<form method=\"post\" id=\"form\">");
                htmlCode.AppendLine("\t\t\t\t\t\t<div class=\"row\">");

                var columns = table.Columns.Where(c => c.UseAsSearchParameter && c.IgnoreOnDTO == false).ToList();
                for( var i=0; i < columns.Count; i++)
                {
                    htmlCode.AppendLine("\t\t\t\t\t\t\t<div class=\"form-group col-sm-6\">");
                    htmlCode.AppendLine("\t\t\t\t\t\t\t\t<label class=\"\">" + System.Web.HttpUtility.HtmlEncode( columns[i].Label ) + ":</label>");
                    htmlCode.AppendLine("\t\t\t\t\t\t\t\t<div>");

                    if( columns[i].UseAsRelatedObject == false )
                    {
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t<input type=\"text\" class=\"form-control\" placeholder=\"" + System.Web.HttpUtility.HtmlEncode(columns[i].Label) + "\" name=\"" + columns[i].DTOName + "\" value=\"@Model." + columns[i].DTOName + "\">");
                    }
                    else
                    {
                        if( columns[i].SelectionType == enumSelectionType.ComboBox )
                        {
                            var relatedTable = tables.Where(t => t.Name == columns[i].RelatedTable).First();
                            var pkColumn = relatedTable.Columns.Where(c => c.IsPK).First();
                            var lbColumn = relatedTable.Columns.Where(c => c.UseAsLabelOnComboBox).First();

                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t@if( ViewBag.LST_" + relatedTable.Alias + " != null )");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t{");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t\tfor( var i=0; i < ViewBag.LST_" + relatedTable.Alias + ".Count; i++)");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t\t{");
                            htmlCode.AppendLine(string.Format("\t\t\t\t\t\t\t\t\t\t\t<option value=\"{0}\">{1}</option>", pkColumn.ColumnName, lbColumn.ColumnName));
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t\t}");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t}");
                        }
                        else
                        {
                            //TODO: Implementar Criacao do Modal no Topo da Página + Chamada no Input
                        }
                    }

                    htmlCode.AppendLine("\t\t\t\t\t\t\t\t</div>");
                    htmlCode.AppendLine("\t\t\t\t\t\t\t</div>");
                }

                htmlCode.AppendLine("\t\t\t\t\t\t</div>");
                htmlCode.AppendLine("\t\t\t\t\t\t<div class=\"row\">");
                htmlCode.AppendLine("\t\t\t\t\t\t\t<div class=\"col-sm-3 col-lg-2 pull-right\">");
                htmlCode.AppendLine("\t\t\t\t\t\t\t\t<label class=\"\">&nbsp;</label>");
                htmlCode.AppendLine("\t\t\t\t\t\t\t\t<button type=\"submit\" class=\"btn btn-primary btn-flat pull-right form-control\" id=\"btn-filtrar\"><i class=\"fa fa-search\"></i>Pesquisar</button>");
                htmlCode.AppendLine("\t\t\t\t\t\t\t</div>");
                htmlCode.AppendLine("\t\t\t\t\t\t</div>");
                htmlCode.AppendLine("\t\t\t\t\t</form>");
                htmlCode.AppendLine("\t\t\t\t</div>");
                htmlCode.AppendLine("\t\t\t</div>");
                htmlCode.AppendLine("\t\t</div>");
                htmlCode.AppendLine("\t</div>");
                htmlCode.AppendLine("");

                #endregion

                #region Table List

                htmlCode.AppendLine("\t@if(Model != null && Model.Count > 0)");
                htmlCode.AppendLine("\t{");
                htmlCode.AppendLine("\t\t<div class=\"row\">");
                htmlCode.AppendLine("\t\t\t<div class=\"col-sm-12\">");
                htmlCode.AppendLine("\t\t\t\t<div class=\"block-flat no-padding\">");
                htmlCode.AppendLine("\t\t\t\t\t<div class=\"content\">");
                htmlCode.AppendLine("\t\t\t\t\t\t<table class=\"no-border red\">");
                htmlCode.AppendLine("\t\t\t\t\t\t\t<thead class=\"no-border\">");
                htmlCode.AppendLine("\t\t\t\t\t\t\t\t<tr>");
                
                //Colunas
                var colunas = table.Columns.Where(c => c.ShowOnResultGrid).ToList();
                var pk = table.Columns.Where(c => c.IsPK).First();

                foreach( ColumnModel col in colunas)
                {
                    htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t<th>" + col.Label + "</th>");
                }

                htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t<th>&nbsp;</th>");
                htmlCode.AppendLine("\t\t\t\t\t\t\t\t</tr>");
                htmlCode.AppendLine("\t\t\t\t\t\t\t</thead>");
                htmlCode.AppendLine("\t\t\t\t\t\t\t<tbody class=\"no-border-x\">");
                htmlCode.AppendLine("\t\t\t\t\t\t\t@foreach(var item in Model)");
                htmlCode.AppendLine("\t\t\t\t\t\t\t{");
                htmlCode.AppendLine("\t\t\t\t\t\t\t\t<tr>");
                //Colunas
                foreach (ColumnModel col in colunas)
                {
                    string complemento = "";
                    switch(col.DataType.Replace("?", ""))
                    {
                        case "DateTime":
                            complemento = ".ToString(\"dd/MM/yyyy HH:mm\")";
                            break;

                        case "Guid":
                            complemento = ".ToString(\"D\")";
                            break;

                        case "decimal":
                            complemento = ".ToString(\"N\")";
                            break;
                    }
                    htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t<td>item." + col.ColumnName + complemento + "</td>");
                }

                htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t<td style=\"width: 20px\">");
                htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t\t<a href=\"/" + table.Alias.Replace("DTO", "") + "/defailt/@(item." + pk.ColumnName + ")?migracao=@(Model.selecionaMigracao.migracao.url_curta)\" >< i class=\"fa fa-search-plus pointer\" class=\"tip\" data-toggle=\"tooltip\" data-original-title=\"Ver detalhes\"></i></a>");
                htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t</td>");
                htmlCode.AppendLine("\t\t\t\t\t\t\t\t</tr>");
                htmlCode.AppendLine("\t\t\t\t\t\t\t}");
                htmlCode.AppendLine("\t\t\t\t\t\t\t</tbody>");
                htmlCode.AppendLine("\t\t\t\t\t\t</table>");
                htmlCode.AppendLine("\t\t\t\t\t</div>");
                htmlCode.AppendLine("\t\t\t\t</div>");
                htmlCode.AppendLine("\t\t\t</div>");
                htmlCode.AppendLine("\t\t@Html.Partial(\"_paging\", Model)");
                htmlCode.AppendLine("\t}");
                htmlCode.AppendLine("\telse");
                htmlCode.AppendLine("\t{");
                htmlCode.AppendLine("\t\t<div class=\"row\">");
                htmlCode.AppendLine("\t\t\t<div class=\"col-sm-12\">");
                htmlCode.AppendLine(string.Format("\t\t\t\t<p>Não há {0} disponíveis com o filtro utilizado.</p>", table.Label));
                htmlCode.AppendLine("\t\t\t</div>");
                htmlCode.AppendLine("\t\t</div>");
                htmlCode.AppendLine("\t}");
                #endregion

                htmlCode.AppendLine("</div>");

                htmlCode.AppendLine("@section scripts{");
                htmlCode.AppendLine("\t<script type=\"text/javascript\" >");
                htmlCode.AppendLine("\t\t$(document).ready(function() {");
                htmlCode.AppendLine("\t\t\t_tableScroll.init($('table'));");
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
