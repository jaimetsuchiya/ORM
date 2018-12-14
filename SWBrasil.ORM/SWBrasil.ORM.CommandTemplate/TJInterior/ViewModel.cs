using SWBrasil.ORM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWBrasil.ORM.CommandTemplate.TJInterior
{
    public class ViewModel : CommandBase, ITableTransformation
    {
        public string CommandID { get { return "ViewModelSGDAI"; } }
        public string Description { get { return "Gera as classes de ViewModel no padrão SGDAI!"; } }
        protected string[] created = new string[] { "createdtime", "createusercode" };
        protected string[] changed = new string[] { "updtdtime", "updtusercode" };

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
            get { return null; }
        }

        public override bool useGroupAsFile
        {
            get { return true; }
        }

        public string ApplyTemplate(TableModel table, List<TableModel> tables = null, string textToAppend = null)
        {
            StringBuilder sb = new StringBuilder();

            //Cria as viewModels com os data anotations

            return sb.ToString();
        }
    }
}
