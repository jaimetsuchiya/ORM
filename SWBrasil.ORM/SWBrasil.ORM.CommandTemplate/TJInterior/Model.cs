using SWBrasil.ORM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWBrasil.ORM.CommandTemplate.TJInterior
{
    public class Model : CommandBase, ITableTransformation
    {
        public string CommandID { get { return "ModelSGDAI"; } }
        public string Description { get { return "Gera o Model no padrão SGDAI!"; } }
        protected string[] created = new string[] { "createdtime", "createusercode" };
        protected string[] changed = new string[] { "updtdtime", "updtusercode" };

        public string Extension
        {
            get { return ".sql"; }
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public string Directory
        {
            get { return null; }
        }

        public override bool useGroupAsFile
        {
            get { return true; }
        }

        public string ApplyTemplate(TableModel table, List<TableModel> tables = null, string textToAppend = null)
        {
            _fileName = table.Name.ToUpper();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\tpublic partial class {table.Name}");
            sb.AppendLine("\t{");
            
            foreach (ColumnModel col in table.Columns.OrderBy(x=>x.Position))
                sb.AppendLine($"\t\tpublic {col.DataType} {col.ColumnName} " + "{ get; set; }");

            sb.AppendLine("\t}");
            return sb.ToString();
        }
    }
}
