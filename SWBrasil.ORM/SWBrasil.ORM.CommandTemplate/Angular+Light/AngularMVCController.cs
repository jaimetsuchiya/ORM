using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class AngularMVCController : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "AngularMVCController"; }
        }

        public string Description
        {
            get { return "Cria Controller MVC para Paginas de List e Details Baseada em implementação Angular"; }
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
            _messages.Add(new ProjectConsoleMessages() { erro = false, data = DateTime.Now, mensagem = string.Format("{0} - Processando Tabela [{1}]", this.CommandID, table.Name) });

            string controller = table.Alias.Replace("DTO", "") + "Controller";
            _fileName = controller;

            StringBuilder classCode = new StringBuilder();
            if (table.MainDTO == false )
                    return "";
            var baseConstructorParam = "";
            var dependencyTables = base.GetAllDependencies(table, tables);
            //var relatedTables = base.MainTableRelations(table, tables);
            var variables = "";
            var constructorParameters = "";
            var constructorVariableAssign = "";
            Dependencies(table, dependencyTables, false, out variables, out constructorParameters, out constructorVariableAssign, out baseConstructorParam);

            classCode.AppendLine("using System;");
            classCode.AppendLine("using System.Collections.Generic;");
            classCode.AppendLine("using System.Linq;");
            classCode.AppendLine("using System.Web;");
            classCode.AppendLine("using System.Web.Mvc;");
            classCode.AppendLine("using SwServices.Common.Business;");
            classCode.AppendLine("using SwServices.Common.DTOs;");
            classCode.AppendLine("using Light;");
            classCode.AppendLine(string.Format("using {0}.Data.Models;", this.NameSpace));
            classCode.AppendLine(string.Format("using {0}.Common.Business;", this.NameSpace));
            classCode.AppendLine(string.Format("using {0}.Common.DTOs;", this.NameSpace));
            classCode.AppendLine("");
            classCode.AppendLine("namespace " + this.NameSpace + ".Web.Controllers");
            classCode.AppendLine("{");
            classCode.AppendLine("\t");
            classCode.AppendLine("\tpublic class " + controller + " : Controller");
            classCode.AppendLine("\t{");
            classCode.AppendLine("\t");
            classCode.AppendLine("\t\t#region Variaveis Membro");
            classCode.AppendLine(variables);
            classCode.AppendLine("\t");
            classCode.AppendLine("\t\t#endregion Variaveis Membro");
            classCode.AppendLine("\t");
            classCode.AppendLine("\t");
            classCode.AppendLine("\t\tpublic " + controller + "(" + constructorParameters + "): base()");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine(constructorVariableAssign);
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("\t");
            classCode.AppendLine("\t");
            classCode.AppendLine("\t\t[HttpGet]");
            classCode.AppendLine("\t\tpublic ActionResult List()");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t\treturn View();");
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("");
            classCode.AppendLine("");

            var pk = table.Columns.Where(c => c.IsPK).First();
            
            classCode.AppendLine("\t\t//[Authorize]");
            classCode.AppendLine("\t\t[HttpGet]");
            classCode.AppendLine("\t\tpublic ActionResult Details(" + pk.DataType + " " + pk.DTOName + ")");
            classCode.AppendLine("\t\t{");
            var relatedColumns = table.Columns.Where(c => string.IsNullOrEmpty(c.RelatedTable) == false && c.SelectionType == enumSelectionType.ComboBox && c.IgnoreOnDTO == false).ToList();
            for( var i=0; i < relatedColumns.Count; i++)
            {
                var relatedTable = tables.Where(t => t.Name == relatedColumns[i].RelatedTable).FirstOrDefault();
                var resultLST = "result" + relatedTable.Alias.Replace("DTO", "");
                classCode.AppendLine("\t\t\tvar " + resultLST + " = _" + relatedTable.Alias.Replace("DTO", "") + "BS.Search( new Criteria" + relatedTable.Alias + "() {} );");
                classCode.AppendLine("\t\t\tViewBag.LST_" + relatedTable.Alias.Replace("DTO", "") + " = " + resultLST + ";");
                classCode.AppendLine("");
            }
            classCode.AppendLine("\t\t\tViewBag." + pk.DTOName + " = " + pk.DTOName + ";");
            classCode.AppendLine("");
            classCode.AppendLine("\t\t\treturn View();");
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("");
            classCode.AppendLine("");

            classCode.AppendLine("\t\t[HttpGet]");
            classCode.AppendLine("\t\tpublic ActionResult Load(" + pk.DataType.Replace("?", "") + " id)");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t\tvar result = _" + table.Alias.Replace("DTO", "") + "BS.Get(id);");
            classCode.AppendLine("\t\t\treturn new ContentResult() { ContentType = \"application/json\", Content = Newtonsoft.Json.JsonConvert.SerializeObject(result) };");
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("");
            classCode.AppendLine("");


            classCode.AppendLine("\t\t[HttpPost]");
            classCode.AppendLine("\t\tpublic ActionResult Save(" + table.Alias + " model)");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t\tOutputTransport<" + table.Alias + "> result = new OutputTransport<" + table.Alias + ">();");
            classCode.AppendLine("\t\t\tif (ModelState.IsValid)");
            classCode.AppendLine("\t\t\t{");
            classCode.AppendLine("\t\t\t\tresult = _" + table.Alias.Replace("DTO", "") + "BS.Save(new InputTransport<" + table.Alias + ">() { Data = model, userID = \"\" });");
            classCode.AppendLine("\t\t\t}");
            classCode.AppendLine("\t\t\telse");
            classCode.AppendLine("\t\t\t{");
            classCode.AppendLine("\t\t\t\tstring messages = string.Join(\"; \", ModelState.Values");
            classCode.AppendLine("\t\t\t\t\t\t.SelectMany(x => x.Errors)");
            classCode.AppendLine("\t\t\t\t\t\t.Select(x => x.ErrorMessage));");
            classCode.AppendLine("\t\t\t\tresult.success = false;");
            classCode.AppendLine("\t\t\t\tresult.code = SwServices.Parameters.Default.INVALID_ARGUMENT_CODE;");
            classCode.AppendLine("\t\t\t\tresult.message = SwServices.Parameters.Default.INVALID_ARGUMENT_MESSAGE + \" \" + messages;");
            classCode.AppendLine("\t\t\t}");
            classCode.AppendLine("\t\t\treturn new ContentResult() { ContentType = \"application/json\", Content = Newtonsoft.Json.JsonConvert.SerializeObject(result) };");
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("");
            classCode.AppendLine("");

            classCode.AppendLine("\t}");
            classCode.AppendLine("}");
            classCode.AppendLine("");
            classCode.AppendLine("");


            return classCode.ToString();
        }
    }
}
