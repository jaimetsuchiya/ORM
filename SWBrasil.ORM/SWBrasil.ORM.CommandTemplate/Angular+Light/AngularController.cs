using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class AngularJSController : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "AngularJSController"; }
        }

        public string Description
        {
            get { return "Cria Controller JavaScript para Paginas de List e Details Baseada em implementação Angular"; }
        }

        public string Extension
        {
            get { return ".js"; }
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

            string controller = DeCapitalize(table.Alias.Replace("DTO", "") + "Controller");
            _fileName = controller;

            StringBuilder jsCode = new StringBuilder();
            if (table.MainDTO == false )
                    return "";

            string service = DeCapitalize(controller.Replace("Controller", "SearchWidgetService"));
            string baseUrl = "/" + table.Alias.Replace("DTO", "") + "/Details?id=";
            string pk = table.Columns.Where(c => c.IsPK == true).First().DTOName;

            jsCode.AppendLine("App.controller(\"" + controller + "\", function($scope, $http, " + service + ") {");
            jsCode.AppendLine("");
            jsCode.AppendLine("\t$scope.multipleSelection = [];");
            jsCode.AppendLine("\t$scope.app = null;");

            jsCode.AppendLine("\t$scope.details = function() {");
            jsCode.AppendLine("\t\tif ($scope.multipleSelection != null && $scope.multipleSelection.length == 1)");
            jsCode.AppendLine("\t\t\tdocument.location.href = \"" + baseUrl + "\" + $scope.multipleSelection[0]." + pk + ";");
            jsCode.AppendLine("\t};");
            jsCode.AppendLine("");
            jsCode.AppendLine("");

            jsCode.AppendLine("\t$scope.$watch(function() { return " + service + ".getSelecteds(); }, function(data) {");
            jsCode.AppendLine("");
            jsCode.AppendLine("\t\t$scope.multipleSelection = " + service + ".getSelecteds();");
            jsCode.AppendLine("\t\t//if ($scope.multipleSelection != null && $scope.multipleSelection.length == 1)");
            jsCode.AppendLine("\t\t//\tdocument.location.href = \"" + baseUrl + "\" + $scope.multipleSelection[0]." + pk + ";");
            jsCode.AppendLine("");
            jsCode.AppendLine("\t}, true);");
            jsCode.AppendLine("");
            jsCode.AppendLine("");

            jsCode.AppendLine("\t$scope.load = function(" + DeCapitalize(pk) + ") {");
            jsCode.AppendLine("\t");
            jsCode.AppendLine("\t\t$http.get(\"/" + table.Alias.Replace("DTO", "") + "/load?id=\" + " + DeCapitalize(pk) + ").success(function(data) {");
            jsCode.AppendLine("\t\t\tconsole.log('" + table.Alias.Replace("DTO", "") + "/load', data);");
            jsCode.AppendLine("\t\t\t$scope.app = data;");
            jsCode.AppendLine("\t\t\tif (!$scope.app.success) {");
            jsCode.AppendLine("\t\t\t\tshowCallOut(\"danger\", \"Detalhe " + table.Label + "\", \"Retorno inesperado na consulta de " + table.Label + ": \" + $scope.app.code + \"-\" + $scope.app.message);");
            jsCode.AppendLine("\t\t\t\treturn;");
            jsCode.AppendLine("\t\t\t}");
            jsCode.AppendLine("\t\t});");
            jsCode.AppendLine("\t}");
            jsCode.AppendLine("");
            jsCode.AppendLine("");

            jsCode.AppendLine("\t$scope.save = function(model)");
            jsCode.AppendLine("\t{");
            jsCode.AppendLine("");
            jsCode.AppendLine("\t\t$http.post(\"/" + table.Alias.Replace("DTO", "") + "/save\", model).success(function(data) {");
            jsCode.AppendLine("\t\t\tconsole.log('" + table.Alias.Replace("DTO", "") + "/save', data);");
            jsCode.AppendLine("\t\t\t$scope.app = data;");
            jsCode.AppendLine("\t\t\t\tif (!$scope.app.success) {");
            jsCode.AppendLine("\t\t\t\t\tshowCallOut(\"danger\", \"Detalhe " + table.Label + "\", \"Retorno inesperado ao Salvar " + table.Label + ": \" + $scope.app.code + \" - \" + $scope.app.message);");
            jsCode.AppendLine("\t\t\t\t\treturn;");
            jsCode.AppendLine("\t\t\t\t}");
            jsCode.AppendLine("\t\t});");
            jsCode.AppendLine("\t}");
            jsCode.AppendLine("");
            jsCode.AppendLine("");

            jsCode.AppendLine("});");
            jsCode.AppendLine("App.factory('" + service + "', function() {");
            jsCode.AppendLine("\tvar collection = [];");
            jsCode.AppendLine("\tvar selected = null;");
            jsCode.AppendLine("\tvar multipleSelection = [];");
            jsCode.AppendLine("");
            jsCode.AppendLine("\treturn { ");
            jsCode.AppendLine("\t\tgetCollection: function() {");
            jsCode.AppendLine("\t\t\tconsole.log('getCollection', collection);");
            jsCode.AppendLine("\t\t\treturn collection;");
            jsCode.AppendLine("\t\t},");
            jsCode.AppendLine("\t\tsetCollection: function(value) {");
            jsCode.AppendLine("\t\t\tconsole.log('setCollection', value);");
            jsCode.AppendLine("\t\t\tcollection = value;");
            jsCode.AppendLine("\t\t},");
            jsCode.AppendLine("\t\tgetSelected: function() {");
            jsCode.AppendLine("\t\t\tconsole.log('getSelected', selected);");
            jsCode.AppendLine("\t\t\treturn selected;");
            jsCode.AppendLine("\t\t},");
            jsCode.AppendLine("\t\tsetSelected: function(value) {");
            jsCode.AppendLine("\t\t\tconsole.log('setSelected', value);");
            jsCode.AppendLine("\t\t\tselected = value;");
            jsCode.AppendLine("\t\t},");
            jsCode.AppendLine("\t\tgetSelecteds: function() {");
            jsCode.AppendLine("\t\t\tconsole.log('getSelecteds', multipleSelection);");
            jsCode.AppendLine("\t\t\treturn multipleSelection;");
            jsCode.AppendLine("\t\t},");
            jsCode.AppendLine("\t\tsetSelecteds: function(value) {");
            jsCode.AppendLine("\t\t\tconsole.log('setSelecteds', value);");
            jsCode.AppendLine("\t\t\tmultipleSelection = value;");
            jsCode.AppendLine("\t\t},");
            jsCode.AppendLine("\t};");
            jsCode.AppendLine("});");
            jsCode.AppendLine("");

            return jsCode.ToString();
        }
    }
}
