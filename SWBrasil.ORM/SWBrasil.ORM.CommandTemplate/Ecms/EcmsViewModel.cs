using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class EcmsViewModel : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "EcmsViewModel"; }
        }

        public string Description
        {
            get { return "Cria Model´s de Persistência no Padrão Napier Ecms"; }
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
            _fileName = table.ModelName.Replace("Model", "");

            StringBuilder classCode = new StringBuilder();

            classCode.AppendLine("using System;");
            classCode.AppendLine("using System.Collections.Generic;");
            classCode.AppendLine("using System.ComponentModel.DataAnnotations;");
            classCode.AppendLine("using System.Linq;");
            classCode.AppendLine("using System.Web;");
            classCode.AppendLine("");
            classCode.AppendLine("namespace Webstore.Ecms.Models");
            classCode.AppendLine("{");
            classCode.AppendLine("");
            classCode.AppendLine("\tpublic class " + table.ModelName + " : WebStore.Sites.Core." + table.ModelName);
            classCode.AppendLine("\t{");
            foreach (ColumnModel col in table.Columns)
            {
                string isPK = col.IsPK ? ", IsKey = true" : "";
                string isIdentity = col.IsIdentity ? ", IsIdentity = true" : "";

                //if (col.IsPK)
                //    col.DataType = col.DataType;

                if( string.IsNullOrEmpty(col.RelatedTable) == false )
                {
                    var relatedTable = tables.Where(t => t.Name == col.RelatedTable).First();

                }
                else
                {
                    classCode.AppendLine(string.Format("\t\t[MapperAttribute(Name = \"{0}\" {1} {2})]", col.ColumnName, isPK, isIdentity));
                    classCode.AppendLine(string.Format("\t\tDisplay(Name = \"{0}\")", col.Label));
                    if (col.DataType == "string")
                        classCode.AppendLine(string.Format("\t\tMaxLength({0}, ErrorMessage=\"{1} deve conter no máximo {2} caracteres\")", col.Label, col.Label, col.Size.ToString()));
                    classCode.AppendLine(string.Format("\t\tpublic override {0} {1}", col.DataType, col.ColumnName) + " { get; set; }");
                }

                classCode.AppendLine("");
            }
            classCode.AppendLine("");
            classCode.AppendLine("\t}");
            classCode.AppendLine("}");

            return classCode.ToString();
        }

    }
}
