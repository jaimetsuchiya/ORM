using SWBrasil.ORM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWBrasil.ORM.CommandTemplate.TJInterior
{
    public class Controller : CommandBase, ITableTransformation
    {
        public string CommandID { get { return "ControllerSGDAI"; } }
        public string Description { get { return "Gera as classes de Controller no padrão SGDAI!"; } }
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

            //Recuperar as tabelas envolvidas no processo (related tables)
            //Criar a injeção de dependência
            //Criar o construtor
            //Criar o método index
            //Criar os métodos de Mapper Model -> ViewModel e ViewModel -> Model

            return sb.ToString();
        }
    }
}
