using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class JQueryDataTables : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "JQuery DataTables"; }
        }

        public string Description
        {
            get { return "Cria DataGrid em HTML + JavaScript"; }
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
            _fileName = table.Name.Replace("tb_", "")+"Grid";

            StringBuilder ret = new StringBuilder();
            string gridBody = "<div class=\"panel panel-default\"><div class=\"panel-heading\"><h3 class=\"panel-title\">Registro(s) encontrado(s):</h3><div class=\"btn-group pull-right\"><button class=\"btn btn-danger dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\"><i class=\"fa fa-bars\"></i> Export Data</button><ul class=\"dropdown-menu\"><li><a href=\"#\" onclick=\"$('#{0}').tableExport({type:'json',escape:'false'});\"><img src=\"/Content/atlant/img/icons/json.png\" width=\"24\"> JSON</a></li><li><a href=\"#\" onclick=\"$('#{0}').tableExport({type:'json',escape:'false',ignoreColumn:'[2,3]'});\"><img src=\"/Content/atlant/img/icons/json.png\" width=\"24\"> JSON (ignoreColumn)</a></li><li><a href=\"#\" onclick=\"$('#{0}').tableExport({type:'json',escape:'true'});\"><img src=\"/Content/atlant/img/icons/json.png\" width=\"24\"> JSON (with Escape)</a></li><li class=\"divider\"></li><li><a href=\"#\" onclick=\"$('#{0}').tableExport({type:'xml',escape:'false'});\"><img src=\"/Content/atlant/img/icons/xml.png\" width=\"24\"> XML</a></li><li><a href=\"#\" onclick=\"$('#{0}').tableExport({type:'sql'});\"><img src=\"/Content/atlant/img/icons/sql.png\" width=\"24\"> SQL</a></li><li class=\"divider\"></li><li><a href=\"#\" onclick=\"$('#{0}').tableExport({type:'csv',escape:'false'});\"><img src=\"/Content/atlant/img/icons/csv.png\" width=\"24\"> CSV</a></li><li><a href=\"#\" onclick=\"$('#{0}').tableExport({type:'txt',escape:'false'});\"><img src=\"/Content/atlant/img/icons/txt.png\" width=\"24\"> TXT</a></li><li class=\"divider\"></li><li><a href=\"#\" onclick=\"$('#{0}').tableExport({type:'excel',escape:'false'});\"><img src=\"/Content/atlant/img/icons/xls.png\" width=\"24\"> XLS</a></li><li><a href=\"#\" onclick=\"$('#{0}').tableExport({type:'doc',escape:'false'});\"><img src=\"/Content/atlant/img/icons/word.png\" width=\"24\"> Word</a></li><li><a href=\"#\" onclick=\"$('#{0}').tableExport({type:'powerpoint',escape:'false'});\"><img src=\"/Content/atlant/img/icons/ppt.png\" width=\"24\"> PowerPoint</a></li><li class=\"divider\"></li><li><a href=\"#\" onclick=\"$('#{0}').tableExport({type:'png',escape:'false'});\"><img src=\"/Content/atlant/img/icons/png.png\" width=\"24\"> PNG</a></li><li><a href=\"#\" onclick=\"$('#{0}').tableExport({type:'pdf',escape:'false'});\"><img src=\"/Content/atlant/img/icons/pdf.png\" width=\"24\"> PDF</a></li></ul></div></div><div class=\"panel-body\"><table class=\"table .table-hover\" id=\"{0}\" role=\"grid\" ></table></div></div>";
            ret.Append(gridBody.Replace("{0}", _fileName));

            StringBuilder js = new StringBuilder();
            js.AppendLine("var " + _fileName + "Grid = {");

            js.AppendLine("\tServiceURI: '',");
            js.AppendLine("\tLinkURI: '',");
            js.AppendLine("\tMultiplos: false,");
            js.AppendLine("\tExibirLink: false,");
            js.AppendLine("\tDados: [],");
            js.AppendLine("\tAjustarColunas: true,");

            js.AppendLine("\tSelecionados: function () {");
            js.AppendLine("\t\tvar arr = [];");
            js.AppendLine("\t\t$('.chk" + _fileName + "').each(function () {");
            js.AppendLine("\t\t\tif ($(this).prop('checked'))");
            js.AppendLine("\t\t\t\tarr.push($(this).val());");
            js.AppendLine("\t\t});");
            js.AppendLine("\t\treturn arr;");
            js.AppendLine("\t},");

            js.AppendLine("\tPesquisar: function (param) {");
            js.AppendLine("\t");
            js.AppendLine("\t\t$.ajax({");
            js.AppendLine("\t\t\turl: " + _fileName + "Grid.ServiceURI,");
            js.AppendLine("\t\t\ttype: 'GET',");
            js.AppendLine("\t\t\tdatatype: 'json',");
            js.AppendLine("\t\t\tcontentType: 'application/json; charset=utf-8',");
            js.AppendLine("\t\t\tasync: false,");
            js.AppendLine("\t\t\theaders: {");
            js.AppendLine("\t\t\t\t'Authorization-Token': currentToken,");
            js.AppendLine("\t\t\t\t'Device-ID': currentDevice");
            js.AppendLine("\t\t\t},");
            js.AppendLine("\t\t\tsuccess: function (retorno) {");
            js.AppendLine("\t\t\t\t" + _fileName + "Grid.Data = [];");
            js.AppendLine("\t\t\t\tif (retorno.Code == 0) {");
            js.AppendLine("\t\t\t\t\tfor (var i = 0; i < retorno.Data.length; i++) {");
            js.AppendLine("\t\t\t\t\t\tvar tmp = [];");

            foreach (ColumnModel col in table.Columns.OrderByDescending(c=>c.IsPK).OrderByDescending(c=>c.ExtendedProperty).OrderBy(c=>c.ColumnName))
            {
                if( col.DataType == "DateTime" )
                    js.AppendLine("\t\t\t\t\t\t\ttmp.push(retorno.Data[i]." + col.ColumnName + " != null ? retorno.Data[i]." + col.ColumnName + ".formatarData('dd/mm hh:mm') : '');");

                else if( string.IsNullOrEmpty(col.RelatedTable) == false )
                {
                    TableModel rt = RelatedTable(col, tables);
                    if( rt != null )
                    {
                        if( rt.Columns.Where(c => col.ExtendedProperty == "Label").Count() == 1 )
                            js.AppendLine("\t\t\t\t\t\t\ttmp.push(retorno.Data[i]." + rt.Name.Replace("tb_", "") + "." + rt.Columns.Where(c => col.ExtendedProperty == "Label").First().ColumnName + ");");
                        else
                            js.AppendLine("\t\t\t\t\t\t\ttmp.push(retorno.Data[i]." + rt.Name.Replace("tb_", "") + "." + rt.Columns.FirstOrDefault().ColumnName + ");");
                    }
                    else
                        js.AppendLine("\t\t\t\t\t\t\ttmp.push(retorno.Data[i]." + col.ColumnName + " != null ? retorno.Data[i]." + col.ColumnName + ");");
                }
                else
                    js.AppendLine("\t\t\t\t\t\t\ttmp.push(retorno.Data[i]." + col.ColumnName + " != null ? retorno.Data[i]." + col.ColumnName + ");");
            }
            js.AppendLine("\t\t\t\t\t\t" + _fileName + "Grid.Dados.push(tmp);");
            js.AppendLine("\t\t\t\t\t}");
            js.AppendLine("");
            js.AppendLine("\t\t\t\t\t$('#" + _fileName +"').DataTable({");
            js.AppendLine("\t\t\t\t\t\t\"destroy\": true,");
            js.AppendLine("\t\t\t\t\t\t\"data\": " + _fileName + "Grid.Data,");
            js.AppendLine("\t\t\t\t\t\t\"columns\": [");

            foreach (ColumnModel col in table.Columns.OrderByDescending(c=>c.IsPK).OrderByDescending(c=>c.ExtendedProperty).OrderBy(c=>c.ColumnName))
            {
                if( col.IsPK )
                    js.AppendLine("\t\t\t\t\t\t\t{ \"title\": (" + _fileName + "Grid.Multiplos ? \"<input type='checkbox' id='chk" + _fileName + "'></input>\" : \"#\"), 'sortable': false },");
                else
                    js.AppendLine("\t\t\t\t\t\t\t{ \"title\": \"" +col.ColumnName + "\" },");
            }
            js.AppendLine("\t\t\t\t\t\t],");
            js.AppendLine("\t\t\t\t\t\t\"createdRow\": function (row, data, index) {");
            js.AppendLine("\t\t\t\t\t\t\tif( " + _fileName + "Grid.Multiplos )");
            js.AppendLine("\t\t\t\t\t\t\t\t$('td:eq(0)', row).html('<input type=\"checkbox\" name=\"checkbox\" class=\"chk" + _fileName + "\" value=\"' + data[0] + '\" style=\"width:20px;\" />');");
            js.AppendLine("\t\t\t\t\t\t\telse");
            js.AppendLine("\t\t\t\t\t\t\t\t$('td:eq(0)', row).html('<input type=\"radio\" name=\"checkbox\" class=\"chk" + _fileName + "\" value=\"' + data[0] + '\"  style=\"width:20px;\" />');");
            js.AppendLine("\t\t\t\t\t\t\tif(" + _fileName + "Grid.ExibirLink)");
            js.AppendLine("\t\t\t\t\t\t\t\t$('td:eq(1)', row).html('<a href=\"' + " + _fileName + ".LinkURI + data[0] + '\">' + data[1] + '</a>');");
            js.AppendLine("\t\t\t\t\t\t},");
            js.AppendLine("\t\t\t\t\t\t\"scrollY\": 200,");
            js.AppendLine("\t\t\t\t\t\t\"scrollX\": true");
            js.AppendLine("\t\t\t\t\t});");
            js.AppendLine("");
            js.AppendLine("\t\t\t\t\tif( " + _fileName + "Grid.AjustarColunas == true ) {");
            js.AppendLine("\t\t\t\t\t\tsetTimeout(function () {");
            js.AppendLine("\t\t\t\t\t\t\tvar tables = $.fn.dataTable.tables(true);");
            js.AppendLine("\t\t\t\t\t\t\t$(tables).DataTable().columns.adjust();");
            js.AppendLine("\t\t\t\t\t\t\t" + _fileName + "Grid.AjustarColunas = false;");
            js.AppendLine("\t\t\t\t\t\t}, 10);");
            js.AppendLine("\t\t\t\t\t}");
            js.AppendLine("\t\t\t}");
            js.AppendLine("\t\t\telse {");
            js.AppendLine("\t\t\t\talert(retorno.Message);");
            js.AppendLine("\t\t\t}");
            js.AppendLine("\t\t},");
            js.AppendLine("\t\terror: function (retorno, ajaxOptions, thrownError) {");
            js.AppendLine("\t\t\talert(\"Ocorreu um erro no processamento da sua solicitação! Favor tentar novamente!\");");
            js.AppendLine("\t\t}");
            js.AppendLine("\t});");
            js.AppendLine("}");
            js.AppendLine("}");

            ret.AppendLine("<script type=\"text/javascript\">");
            ret.AppendLine(js.ToString());
            ret.AppendLine("</script>");
            return ret.ToString();
        }
    }
}
