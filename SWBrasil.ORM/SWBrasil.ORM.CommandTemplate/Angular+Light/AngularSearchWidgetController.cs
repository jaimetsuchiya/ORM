using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class AngularSearchWidgetController : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "AngularSearchWidgetController"; }
        }

        public string Description
        {
            get { return "Cria Widget Controller para Pesquisa Baseada em implementação Angular"; }
        }

        public string Extension
        {
            get { return ".cs"; }
        }

        public string FileName
        {
            get { return _fileName + ""; }
        }

        public string Directory
        {
            get { return _directoryName; }
        }


        public string ApplyTemplate(TableModel table, List<TableModel> tables = null, string textToAppend = null)
        {
            _messages.Add(new ProjectConsoleMessages() { erro = false, data = DateTime.Now, mensagem = string.Format("{0} - Processando Tabela [{1}]", this.CommandID, table.Name) });

            _fileName = "WidgetsController";
            _directoryName = this.ProjectName + ".Web\\Views\\Widgets";

            StringBuilder classCode = new StringBuilder();
            StringBuilder variablesCode = new StringBuilder();
            StringBuilder constructorAssignCode = new StringBuilder();
            StringBuilder constructorParamCode = new StringBuilder();

            classCode.AppendLine("// GET: Widget");
            classCode.AppendLine("using System;");
            classCode.AppendLine("using System.Collections.Generic;");
            classCode.AppendLine("using System.Linq;");
            classCode.AppendLine("using System.Web;");
            classCode.AppendLine("using System.Web.Mvc;");
            classCode.AppendLine("using SwServices.Common.Business;");
            classCode.AppendLine("using Light;");
            classCode.AppendLine(string.Format("using {0}.Data.Models;", this.NameSpace));
            classCode.AppendLine(string.Format("using {0}.Common.DTOs;", this.NameSpace));
            classCode.AppendLine(string.Format("using {0}.Common.Business;", this.NameSpace));
            classCode.AppendLine("");
            classCode.AppendLine("namespace " + this.NameSpace + ".Web.Controllers");
            classCode.AppendLine("{");
            classCode.AppendLine("\tpublic class WidgetsController : Controller");
            classCode.AppendLine("\t{");
            classCode.AppendLine("\t");
            classCode.AppendLine("\t#region Variaveis Membro");
            classCode.AppendLine("[VARIAVEIS]");
            classCode.AppendLine("\t#endregion Variaveis Membro");
            classCode.AppendLine("\t");
            classCode.AppendLine("\tpublic WidgetsController([CONSTRUTOR_PARAMETROS])");
            classCode.AppendLine("\t{");
            classCode.AppendLine("[CONSTRUTOR_ASSIGN]");
            classCode.AppendLine("\t}");
            classCode.AppendLine("\t");

            List<string> dependencies = new List<string>();
            foreach( TableModel tbl in tables )
            {
                bool processar = false;
                if( tbl.MainDTO )
                {
                    processar = true;
                }
                else if (tbl.Type != enumTableType.Relation_1_To_N)
                {
                    for (var i = 0; i < tables.Count; i++)
                    {
                        if (tables[i].Columns.Where(c => c.RelatedTable == table.Name && c.SelectionType == enumSelectionType.SearchModal).Count() > 0)
                        {
                            processar = true;
                            break;
                        }
                    }
                }
                if (tbl.Type == enumTableType.Relation_1_To_N)
                {
                    if (tbl.Columns.Where(c => c.IgnoreOnDTO == false).Count() > 1)
                        processar = true;
                }
                

                if( tbl.MainDTO )
                {
                    string controller = tbl.Alias.Replace("DTO", "") + "SearchWidget";
                    string service = controller + "Service";

                    variablesCode.AppendLine("\tprotected I{0} _{0}BS = null;".Replace("{0}", tbl.Alias.Replace("DTO", "")));

                    constructorParamCode.Append(constructorParamCode.ToString() == "" ? "" : ", ");
                    constructorParamCode.Append("I{0} i{0}".Replace("{0}", tbl.Alias.Replace("DTO", "")));
                    constructorAssignCode.AppendLine("\t_{0}BS = i{0};".Replace("{0}", tbl.Alias.Replace("DTO", "")));

                    dependencies.Add("I{0}".Replace("{0}", tbl.Alias.Replace("DTO", "")));

                    classCode.AppendLine("\t\t[HttpGet]");
                    classCode.AppendLine("\t\tpublic ActionResult " + controller + "()");
                    classCode.AppendLine("\t\t{");
                    var columns = tbl.Columns.Where(c => string.IsNullOrEmpty(c.RelatedTable) == false && c.SelectionType == enumSelectionType.ComboBox).ToList();
                    for (var i = 0; i < columns.Count; i++)
                    {
                        var relatedTable = tables.Where(t => t.Name == columns[i].RelatedTable).Single();
                        classCode.AppendLine("\t\t\tvar result" + i.ToString("00") + " = _" + relatedTable.Alias.Replace("DTO", "") + "BS.Search(new Criteria" + relatedTable.Alias + "() {});");
                        classCode.AppendLine("\t\t\tViewBag.LST_" + relatedTable.Alias + " = result" + i.ToString("00") + ".Data;");
                    }
                    classCode.AppendLine("\t\t\treturn View();");
                    classCode.AppendLine("\t\t}");
                    classCode.AppendLine("");
                    classCode.AppendLine("");

                    classCode.AppendLine("\t\t//[Authorize]");
                    classCode.AppendLine("\t\t[HttpPost]");
                    classCode.AppendLine("\t\tpublic ActionResult " + controller + "(Criteria" + tbl.Alias + " model, int currentPage = 1, int pageSize = 10)");
                    classCode.AppendLine("\t\t{");
                    classCode.AppendLine("\t\t\tvar result = _" + tbl.Alias.Replace("DTO", "") + "BS.Search(model, currentPage, pageSize);");
                    classCode.AppendLine("\t\t\treturn Content(Newtonsoft.Json.JsonConvert.SerializeObject(result), \"application /json\");");
                    //htmlCode.AppendLine("\t\t\treturn new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };");
                    classCode.AppendLine("\t\t}");
                }
                
            }
            classCode.AppendLine("");
            classCode.AppendLine("");
            classCode.AppendLine("\t}");
            classCode.AppendLine("}");

            return classCode.ToString().Replace("[CONSTRUTOR_PARAMETROS]", constructorParamCode.ToString()).Replace("[CONSTRUTOR_ASSIGN]", constructorAssignCode.ToString()).Replace("[VARIAVEIS]", variablesCode.ToString());
        }
    }
}
