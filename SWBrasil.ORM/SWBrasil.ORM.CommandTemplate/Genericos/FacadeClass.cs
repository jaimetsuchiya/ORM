using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class FacadeClass : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "FacadeClass"; }
        }

        public string Description
        {
            get { return "Cria Classes de Fachada para os métodos CRUD"; }
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
            string className = table.Group;
            string DTO = table.Name.Replace("tb_", "") + "DTO";
            _fileName = className;
            _directoryName = this.ProjectName + ".Core\\Facade\\" + table.Group;

            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(textToAppend) )
            {
                sb.AppendLine("public class " + className + ": " + className + "Base, I" + className);
                sb.AppendLine("{");
                sb.AppendLine("\tpublic " + className + "(ILogger loggerBO): base(loggerBO)");
                sb.AppendLine("\t{");
                sb.AppendLine("\t}");
                sb.AppendLine("");
            }
            sb.AppendLine("");
            sb.AppendLine("\t#region " + DTO);
            
            sb.AppendLine("\t#endregion CRUD " + DTO);
            sb.AppendLine("");
            if (string.IsNullOrEmpty(textToAppend))
            {
                sb.AppendLine("}");
                return sb.ToString();
            }
            else
            {
                sb.AppendLine("");
                sb.AppendLine("");
                sb.AppendLine("\t");
                int pos = textToAppend.IndexOf("#region");
                return textToAppend.Insert(pos, sb.ToString());
            }
        }


    }
}

