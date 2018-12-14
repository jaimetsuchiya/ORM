using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class AngularSearchWidget : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "AngularSearchWidget"; }
        }

        public string Description
        {
            get { return "Cria Widget View para Pesquisa Baseada em implementação Angular"; }
        }

        public string Extension
        {
            get { return ".cshtml"; }
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

            string controller = DeCapitalize(table.Alias.Replace("DTO", "") + "SearchWidget");
            string service = controller + "Service";
            string formName = "frm" + controller;
            string checkbox = "chk" + table.Alias.Replace("DTO", "") + "SearchWidget";

            string searchButton = "btn" + controller;
            _fileName = table.Alias.Replace("DTO", "") + "SearchWidget";
            _directoryName = this.ProjectName + ".Web\\Views\\Widgets";

            if (table.MainDTO == false && table.Type != enumTableType.Relation_1_To_N)
            {
                bool processar = false;
                for( var i=0; i < tables.Count; i++ )
                {
                    if( tables[i].Columns.Where(c=>c.RelatedTable == table.Name && c.SelectionType == enumSelectionType.SearchModal).Count() > 0)
                    {
                        processar = true;
                        break;
                    }
                }
                if (processar == false)
                    return "";
            }
            if( table.Type == enumTableType.Relation_1_To_N )
            {
                if (table.Columns.Where(c => c.IgnoreOnDTO == false).Count() == 1)
                    return "";
            }

            StringBuilder htmlCode = new StringBuilder();
            htmlCode.AppendLine("@{");
            htmlCode.AppendLine("\tLayout = null;");
            htmlCode.AppendLine("}");
            htmlCode.AppendLine("<div ng-controller=\"" + controller + "\" >");
            htmlCode.AppendLine("\t<div class=\"row\">");
            htmlCode.AppendLine("\t\t<div class=\"col-sm-12\">");
            htmlCode.AppendLine("\t\t\t<div class=\"box box-info\">");
            htmlCode.AppendLine("\t\t\t\t<div class=\"box-header with-border\">");
            htmlCode.AppendLine("\t\t\t\t\t<h3 class=\"box-title\">" + System.Web.HttpUtility.HtmlEncode("Critérios de Pesquisa") + "</h3>");
            htmlCode.AppendLine("\t\t\t\t\t<div class=\"box-tools pull-right\"><button type=\"button\" class=\"btn btn-box-tool\" data-widget=\"collapse\"><i class=\"fa fa-minus\"></i></button></div>");
            htmlCode.AppendLine("\t\t\t\t</div>");
            htmlCode.AppendLine("\t\t\t\t<form class=\"form-horizontal\" name=\"" + formName + "\">");
            htmlCode.AppendLine("\t\t\t\t\t<div class=\"box-body\">");

            //[Campos para Pesquisa]
            foreach (ColumnModel col in table.Columns.Where(c=>c.UseAsSearchParameter).OrderBy(c=>c.Position).ToList())
            {
                if (string.IsNullOrEmpty(col.RelatedTable) == false)
                {
                    if (col.SelectionType == enumSelectionType.ComboBox)
                    {
                        var relatedTable = tables.Where(t => t.Name == col.RelatedTable).Single();
                        var labelColumn = relatedTable.Columns.Where(c => c.UseAsLabelOnComboBox).SingleOrDefault();
                        var pkColumn = relatedTable.Columns.Where(c => c.IsPK).SingleOrDefault();

                        if( labelColumn == null )
                        {
                            _messages.Add(new ProjectConsoleMessages() { data = DateTime.Now, erro = true, mensagem = string.Format("Tabela [{0}] nao possui coluna marcada como label para combobox", col.RelatedTable)});
                            continue;
                        }
                        if (pkColumn == null)
                        {
                            _messages.Add(new ProjectConsoleMessages() { data = DateTime.Now, erro = true, mensagem = string.Format("Tabela [{0}] nao possui coluna marcada como PK", col.RelatedTable) });
                            continue;
                        }

                        htmlCode.AppendLine("\t\t\t\t\t\t<div class=\"col-sm-6 col-md-4 col-lg-3 col-xs-12\">");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t<div class=\"form-group\">");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t<label for=\"txt" + col.DTOName + "\">" + System.Web.HttpUtility.HtmlEncode(col.Label) + "</label>");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t<select class=\"form-control\" ng-model=\"criteriaDTO." + col.DTOName + "\">");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t@{");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t\tif( ViewBag.LST_" + relatedTable.Alias + " != null )");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t{");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t\tfor( var i=0; i < ViewBag.LST_" + relatedTable.Alias + ".Count; i++)");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t\t{");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t\t\t<option value=\"@(ViewBag.LST_" + relatedTable.Alias + "[i]." + pkColumn.DTOName + ")\">@(ViewBag.LST_" + relatedTable.Alias + "[i]." + labelColumn.DTOName + ")</option>");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t\t}");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t\t}");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t}");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t\t</select>");
                        htmlCode.AppendLine("\t\t\t\t\t\t\t</div>");
                        htmlCode.AppendLine("\t\t\t\t\t\t</div>");
                    }
                    else
                    {
                        //TODO: A implementar a chamada do PopUp de Pesquisa
                    }
                }
                else
                {
                    switch( col.DataType )
                    {
                        case "int":
                        case "long":
                        case "decimal":
                            htmlCode.AppendLine("\t\t\t\t\t\t<div class=\"col-sm-6 col-md-4 col-lg-3 col-xs-12\">");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t<div class=\"form-group\">");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<label for=\"txt" + col.DTOName + "\">" + System.Web.HttpUtility.HtmlEncode(col.Label) + "</label>");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<input type=\"number\" class=\"form-control\" id=\"txt" + col.DTOName + "\" ng-model=\"criteriaDTO." + col.DTOName + "\" ng-required=\"false\" />");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t</div>");
                            htmlCode.AppendLine("\t\t\t\t\t\t</div>");
                            break;

                        case "bool":
                            htmlCode.AppendLine("\t\t\t\t\t\t<div class=\"col-sm-6 col-md-4 col-lg-3 col-xs-12\">");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t<div class=\"form-group\">");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<label for=\"txt" + col.DTOName + "\">" + System.Web.HttpUtility.HtmlEncode(col.Label) + "</label>");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<select class=\"form-control\" ng-model=\"criteriaDTO." + col.DTOName + "\" ng-options=\"(item?'Sim':'Não') for item in [true, false]\"></select>");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t</div>");
                            htmlCode.AppendLine("\t\t\t\t\t\t</div>");
                            break;

                        case "DateTime":
                            htmlCode.AppendLine("\t\t\t\t\t\t<div class=\"col-sm-6 col-md-4 col-lg-3 col-xs-12\">");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t<div class=\"form-group\">");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<label for=\"txt" + col.DTOName + "\">" + System.Web.HttpUtility.HtmlEncode(col.Label) + " Inicial</label>");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<input type=\"text\" class=\"form-control\" id=\"txt" + col.DTOName + "Start\" ng-model=\"criteriaDTO." + col.DTOName + "Start\" ng-required=\"false\" />");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t</div>");
                            htmlCode.AppendLine("\t\t\t\t\t\t</div>");

                            htmlCode.AppendLine("\t\t\t\t\t\t<div class=\"col-sm-6 col-md-4 col-lg-3 col-xs-12\">");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t<div class=\"form-group\">");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<label for=\"txt" + col.DTOName + "\">" + System.Web.HttpUtility.HtmlEncode(col.Label) + " Final</label>");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<input type=\"text\" class=\"form-control\" id=\"txt" + col.DTOName + "Finish\" ng-model=\"criteriaDTO." + col.DTOName + "Finish\" ng-required=\"false\" />");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t</div>");
                            htmlCode.AppendLine("\t\t\t\t\t\t</div>");
                            break;

                        default:
                            htmlCode.AppendLine("\t\t\t\t\t\t<div class=\"col-sm-6 col-md-4 col-lg-3 col-xs-12\">");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t<div class=\"form-group\">");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<label for=\"txt" + col.DTOName + "\">" + System.Web.HttpUtility.HtmlEncode(col.Label) + "</label>");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<input type=\"text\" class=\"form-control\" id=\"txt" + col.DTOName + "\" ng-model=\"criteriaDTO." + col.DTOName + "\" ng-required=\"false\" maxlength=\"" + (col.Size.HasValue && col.Size.Value > 0 ? col.Size.Value.ToString() : "50") + "\" />");
                            htmlCode.AppendLine("\t\t\t\t\t\t\t</div>");
                            htmlCode.AppendLine("\t\t\t\t\t\t</div>");
                            break;
                    }
                }

               
            }

            htmlCode.AppendLine("\t\t\t\t\t</div>");
            htmlCode.AppendLine("\t\t\t\t\t<div class=\"box-footer\">");
            htmlCode.AppendLine("\t\t\t\t\t\t<button type=\"button\" class=\"btn btn-info pull-right\" ng-click=\"search(criteriaDTO)\" style=\"margin:5px;\" id=\"" + searchButton + "\">Pesquisar</button>");
            htmlCode.AppendLine("\t\t\t\t\t\t<button type=\"button\" class=\"btn btn-default pull-right\" ng-click=\"reset()\" style=\"margin:5px;\">Limpar</button>");
            htmlCode.AppendLine("\t\t\t\t\t</div>");
            htmlCode.AppendLine("\t\t\t\t</form>");
            htmlCode.AppendLine("\t\t\t</div>");
            htmlCode.AppendLine("\t\t</div>");
            htmlCode.AppendLine("\t</div>");
            htmlCode.AppendLine("");

            htmlCode.AppendLine("\t<div class=\"row\">");
            htmlCode.AppendLine("\t\t<div class=\"col-sm-12\">");
            htmlCode.AppendLine("\t\t\t<div class=\"box box-info\">");
            htmlCode.AppendLine("\t\t\t\t<div class=\"box-header with-border\">");
            htmlCode.AppendLine("\t\t\t\t\t<h3 class=\"box-title\">" + table.Alias.Replace("DTO", "") + "</h3>");
            htmlCode.AppendLine("\t\t\t\t</div>");
            htmlCode.AppendLine("\t\t\t\t<div class=\"box-body no-padding\">");
            htmlCode.AppendLine("\t\t\t\t\t<table class=\"table table-striped\">");
            htmlCode.AppendLine("\t\t\t\t\t\t<tr>");
            htmlCode.AppendLine("\t\t\t\t\t\t\t<th style=\"width: 10px\"><input type=\"checkbox\" id=\"" + checkbox + "\"/></th>");

            //<th>Código</th>
            //<th>Referencia</th>
            //<th>Nome</th>
            //<th>Marca</th>
            //<th>Categoria</th>
            foreach (ColumnModel col in table.Columns.Where(c => c.ShowOnResultGrid).OrderBy(c => c.Position).ToList())
            {
                htmlCode.AppendLine(string.Format("\t\t\t\t\t\t\t<th>{0}</th>", System.Web.HttpUtility.HtmlEncode(col.Label)));
            }
            string keyProperty = table.Columns.Where(c => c.IsPK).First().DTOName;
            
            htmlCode.AppendLine("\t\t\t\t\t\t</tr>");
            htmlCode.AppendLine("\t\t\t\t\t\t<tr ng-repeat=\"item in collection\">");
            htmlCode.AppendLine("\t\t\t\t\t\t\t<td><input type='checkbox' class='" + checkbox + "' ng-click=\"select(item);\" value='{{item." + keyProperty + "}}' /></td>");
            //                                <td>{{item.code}}</td>
            //                                <td>{{item.reference}}</td>
            //                                <td>{{item.title}}</td>
            //                                <td>{{item.Brand.name}}</td>
            //                                <td>{{item.Category.name}}</td>
            foreach (ColumnModel col in table.Columns.Where(c => c.ShowOnResultGrid).OrderBy(c=>c.Position).ToList())
            {
                if( string.IsNullOrEmpty(col.RelatedTable) == false )
                {
                    var tmpTable = tables.Where(t => t.Name == col.RelatedTable).First();
                    var labelColumn = tmpTable.Columns.Where(c => c.UseAsLabelOnComboBox).FirstOrDefault();
                    if( labelColumn == null )
                        labelColumn = tmpTable.Columns.Where(c => c.DataType == "string").FirstOrDefault();

                    htmlCode.AppendLine("\t\t\t\t\t\t\t<td>{{item." + tmpTable.Alias.Replace("DTO", "") + "." + labelColumn.DTOName + "}}</td>");
                }
                else
                    htmlCode.AppendLine("\t\t\t\t\t\t\t<td>{{item." + col.DTOName + "}}</td>");
            }
            htmlCode.AppendLine("\t\t\t\t\t\t</tr>");
            htmlCode.AppendLine("\t\t\t\t\t</table>");
            htmlCode.AppendLine("\t\t\t\t</div>");
            htmlCode.AppendLine("\t\t\t\t<div class=\"box-footer clearfix\">");
            htmlCode.AppendLine("\t\t\t\t\t<div class=\"pagination-sm no-margin pull-left\">");
            htmlCode.AppendLine("\t\t\t\t\t\t<span class=\"col-xs-7\" style=\"padding-top:10px;\">Registros/" + System.Web.HttpUtility.HtmlEncode("Página") + ": </span>");
            htmlCode.AppendLine("\t\t\t\t\t\t<div class=\"col-xs-5\">");
            htmlCode.AppendLine("\t\t\t\t\t\t\t<select ng-model=\"pageSize\" class=\"form-control\">");
            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<option value=\"10\" selected>10</option>");
            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<option value=\"20\">20</option>");
            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<option value=\"50\">50</option>");
            htmlCode.AppendLine("\t\t\t\t\t\t\t\t<option value=\"100\">100</option>");
            htmlCode.AppendLine("\t\t\t\t\t\t\t</select>");
            htmlCode.AppendLine("\t\t\t\t\t\t</div>");
            htmlCode.AppendLine("\t\t\t\t\t</div>");
            htmlCode.AppendLine("\t\t\t\t\t<ul class=\"pagination pagination-sm no-margin pull-right\" id=\"pagination\" style=\"width:300px;\">");
            htmlCode.AppendLine("\t\t\t\t\t\t<li><a href=\"#\" ng-click=\"firstPage($event)\"><span class=\"fa fa-fast-backward\"></span></a></li>");
            htmlCode.AppendLine("\t\t\t\t\t\t<li><a href=\"#\" ng-click=\"previousPage($event)\" ><span class=\"fa fa-backward\"></span></a></li>");
            htmlCode.AppendLine("\t\t\t\t\t\t<li><span>{{currentPage}}</span><span> de</span><span>{{pages}}</span></li>");
            htmlCode.AppendLine("\t\t\t\t\t\t<li><a href=\"#\" ng-click=\"nextPage($event)\"><span class=\"fa fa-forward\"></span></a></li>");
            htmlCode.AppendLine("\t\t\t\t\t\t<li><a href=\"#\" ng-click=\"lastPage($event)\"><span class=\"fa fa-fast-forward\"></span></a></li>");
            htmlCode.AppendLine("\t\t\t\t\t</ul>");
            htmlCode.AppendLine("\t\t\t\t</div>");
            htmlCode.AppendLine("\t\t\t</div>");
            htmlCode.AppendLine("\t\t</div>");
            htmlCode.AppendLine("\t</div>");
            htmlCode.AppendLine("</div>");
            htmlCode.AppendLine("");
            htmlCode.AppendLine("<script type=\"text/javascript\" >");
            htmlCode.AppendLine("App.controller(\"" + controller + "\", function($scope, $http, " + service + ") {");
            htmlCode.AppendLine("\t$scope.selected = [];");
            htmlCode.AppendLine("\t$scope.criteriaDTO = { };");
            htmlCode.AppendLine("\t$scope.currentPage = 1;");
            htmlCode.AppendLine("\t$scope.pageSize = 10;");
            htmlCode.AppendLine("\t$scope.pages = -1;");
            htmlCode.AppendLine("\t$scope.collection = [];");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\t$scope.select = function(item) {");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\t\tvar index = $scope.selected.indexOf(item);");
            htmlCode.AppendLine("\t\tif (index < 0)");
            htmlCode.AppendLine("\t\t\t$scope.selected.push(item);");
            htmlCode.AppendLine("\t\telse {");
            htmlCode.AppendLine("\t\t\t$scope.selected = $scope.selected.splice(index, 1);");
            htmlCode.AppendLine("\t\t}");
            htmlCode.AppendLine(string.Format("\t\t{0}.setSelecteds($scope.selected);", service));
            htmlCode.AppendLine("\t}");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\t$scope.reset = function() {");
            htmlCode.AppendLine("\t\t$scope.criteriaDTO = { };");
            htmlCode.AppendLine("\t\t$scope.currentPage = 1;");
            htmlCode.AppendLine("\t\t$scope.pageSize = 10;");
            htmlCode.AppendLine("\t\t$scope.pages = -1;");
            htmlCode.AppendLine("\t\t$scope.collection = [];");
            htmlCode.AppendLine("\t}");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\t$scope.firstPage = function($event) {");
            htmlCode.AppendLine("\t\t$scope.changePage(1, $event);");
            htmlCode.AppendLine("\t}");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\t$scope.lastPage = function($event) {");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\tif ( $scope.pages > 0 )");
            htmlCode.AppendLine("\t\t$scope.changePage($scope.pages, $event);");
            htmlCode.AppendLine("\t}");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\t$scope.previousPage = function($event) {");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\t\tif ($scope.currentPage > 1) {");
            htmlCode.AppendLine("\t\t\t$scope.currentPage = $scope.currentPage - 1;");
            htmlCode.AppendLine("\t\t\t$scope.changePage($scope.currentPage, $event);");
            htmlCode.AppendLine("\t\t}");
            htmlCode.AppendLine("\t}");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\t$scope.nextPage = function($event) {");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\t\tif ($scope.currentPage < $scope.pages) {");
            htmlCode.AppendLine("\t\t\t$scope.currentPage = $scope.currentPage + 1;");
            htmlCode.AppendLine("\t\t\t$scope.changePage($scope.currentPage, $event);");
            htmlCode.AppendLine("\t\t}");
            htmlCode.AppendLine("\t}");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\t$scope.changePage = function(pageNumber, $event) {");
            htmlCode.AppendLine("\t\t");
            htmlCode.AppendLine("\t\t$scope.currentPage = pageNumber;");
            htmlCode.AppendLine("\t\tconsole.log('currentPage', $scope.currentPage);");
            htmlCode.AppendLine("\t\t$scope.search($scope.criteriaDTO);");
            htmlCode.AppendLine("\t}");
            htmlCode.AppendLine("\t");
            htmlCode.AppendLine("\t$scope.search = function(criteriaDTO) {");
            htmlCode.AppendLine("\t\t$http.post((\"/Widgets/" + (table.Alias.Replace("DTO", "") + "SearchWidget") + "?currentPage=\" + $scope.currentPage + \"&pageSize=\" + $scope.pageSize), criteriaDTO).success(function(data) {");
            htmlCode.AppendLine("\t\t\tconsole.log('search{0} result', data);".Replace("{0}", table.Alias.Replace("DTO", "")));
            htmlCode.AppendLine("\t\t\tif (data.success) {");
            htmlCode.AppendLine("\t\t\t\t$scope.collection = data.Data;");
            htmlCode.AppendLine("\t\t\t\t$scope.pages = data.pages;");
            htmlCode.AppendLine("\t\t\t}");
            htmlCode.AppendLine("\t\t\telse {");
            htmlCode.AppendLine("\t\t\t");
            htmlCode.AppendLine("\t\t\t\tshowCallOut(\"danger\", \"Consulta de {0}\", \"Retorno inesperado na consulta de Log: \" + data.code + \"-\" + data.message);".Replace("{0}", table.Label));
            htmlCode.AppendLine("\t\t\t}");
            htmlCode.AppendLine("\t\t");
            htmlCode.AppendLine("\t\t}).error(function(response) { showCallOut(\"danger\", \"Consulta de {0}\", \"Ocorreu um erro na consulta de {0}!\"); });".Replace("{0}", table.Label));
            htmlCode.AppendLine("\t}");
            htmlCode.AppendLine("});");
            htmlCode.AppendLine("");
            htmlCode.AppendLine("");
            htmlCode.AppendLine("$(document).ready(function ()");
            htmlCode.AppendLine("{");
            htmlCode.AppendLine("\tcheckAll(\"" + checkbox + "\");");
            htmlCode.AppendLine("})");
            htmlCode.AppendLine("</script>");

            return htmlCode.ToString();
        }
    }
}
