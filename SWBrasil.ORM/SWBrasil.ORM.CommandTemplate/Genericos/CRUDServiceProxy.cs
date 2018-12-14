using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class ListPage : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "ListPage"; }
        }

        public string Description
        {
            get { return "Cria a Página de Pesquisa"; }
        }

        public string Extension
        {
            get { return ".cs"; }
        }

        public string Directory
        {
            get { return this.NameSpace + ".UI\\App_Start"; }
        }

        public override bool useGroupAsFile
        {
            get { return true; }
        }

        public string ApplyTemplate(TableModel table, List<TableModel> tables = null, string textToAppend = null)
        {
            _fileName = table.Name.Replace("tb_", "");
            if( string.IsNullOrEmpty(table.Group))
                return "";

            return Templates.Default.ListPage.Replace("{GROUP}", table.Group).Replace("{TABLE_NAME}", _fileName).Replace("{NAMESPACE}", base.NameSpace);
        }

        public string FileName
        {
            get { return _fileName; }
        }
    }
}
