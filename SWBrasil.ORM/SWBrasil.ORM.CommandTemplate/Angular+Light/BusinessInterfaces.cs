using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class BusinessInterfaces : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "BusinessInterfaces"; }
        }

        public string Description
        {
            get { return "Cria Interfaces para as Classes de Fachada"; }
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


        public override bool useGroupAsFile
        {
            get { return true; }
        }


        public string ApplyTemplate(TableModel table, List<TableModel> tables = null, string textToAppend = null)
        {
            _messages.Add(new ProjectConsoleMessages() { erro = false, data = DateTime.Now, mensagem = string.Format("{0} - Processando Tabela [{1}]", this.CommandID, table.Name) });

            string className = table.Alias.Replace("DTO", "");
            _fileName = "I" + className + "Persistence";


            if (table.IgnoreDTO == false)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using System.Data;");
                sb.AppendLine("using System.Linq;");
                sb.AppendLine("using System.Text;");
                sb.AppendLine(string.Format("using {0}.Common.DTOs;", this.NameSpace));
                sb.AppendLine("using SwServices.Common.DTOs;");
                sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("namespace " + base.NameSpace + ".Common.Business");
                sb.AppendLine("{");
                //sb.AppendLine("\tpublic interface I" + className);
                //sb.AppendLine("\t{");
                //sb.AppendLine("\t}");
                //sb.AppendLine("");
                //sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("\tpublic interface I" + className + "");
                sb.AppendLine("\t{");
                sb.AppendLine("\t\tOutputTransport<" + table.Alias + "> Save(InputTransport<" + table.Alias + "> model, bool openTransaction = false);");
                sb.AppendLine("");
                sb.AppendLine("\t\tTableTransport<List<" + table.Alias + ">> Search(Criteria" + table.Alias + " args);");
                sb.AppendLine("");
                sb.AppendLine("\t\tTableTransport<List<" + table.Alias + ">> Search(Criteria" + table.Alias + " args, int currentPage, int pageSize);");
                sb.AppendLine("");

                var pkColumn = table.Columns.Where(c => c.IsPK).ToList();
                if (pkColumn != null)
                {
                    string methodGet = "\t\tOutputTransport<" + table.Alias + "> Get(";
                    string methodDel = "\t\tOutputTransport<string> Delete(";
                    for ( var i=0; i < pkColumn.Count; i++ )
                    {
                        methodGet += i > 0 ? ", " : "";
                        methodGet += pkColumn[i].DataType.Replace("?", "") + " " + pkColumn[i].DTOName.ToLowerInvariant();
                        methodDel += i > 0 ? ", " : "";
                        methodDel += pkColumn[i].DataType.Replace("?", "") + " " + pkColumn[i].DTOName.ToLowerInvariant();
                    }
                    sb.AppendLine(methodGet + ");");
                    sb.AppendLine("");

                    sb.AppendLine(methodDel + ");");
                    sb.AppendLine("");
                }

                var uniqueKey = "";
                var uniqueKeyColumns = table.Columns.Where(c => c.IsUniqueKey).ToList();
                if (uniqueKeyColumns != null)
                {
                    for (var i = 0; i < uniqueKeyColumns.Count; i++)
                    {
                        uniqueKey += (uniqueKey != "" ? ", " : "");
                        uniqueKey += uniqueKeyColumns[i].DataType;
                        uniqueKey += " ";
                        uniqueKey += uniqueKeyColumns[i].DTOName;
                    }

                    if(string.IsNullOrEmpty(uniqueKey) == false )
                    { 
                        sb.AppendLine("\t\tOutputTransport<" + table.Alias + "> Get(" + uniqueKey + ");");
                        sb.AppendLine("");
                    }
                }
                sb.AppendLine("\t}");
                sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("}");

                return sb.ToString();
            }
            else
                return "";
        }
    }
}

