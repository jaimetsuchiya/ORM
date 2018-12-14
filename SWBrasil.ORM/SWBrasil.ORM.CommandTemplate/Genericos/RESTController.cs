using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class RESTController : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "RESTCommand"; }
        }

        public string Description
        {
            get { return "Cria os métodos básicos de um controller REST"; }
        }

        public string Extension
        {
            get { return ".cs"; }
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
            _fileName = table.Group;
            _directoryName = this.NameSpace + ".Service\\Controllers";

            if( string.IsNullOrEmpty(table.Group))
                return "";

            string ret = Templates.Default.RESTController.Replace("{GROUP}", table.Group).Replace("{TABLE_NAME}", table.Name.Replace("tb_", "")).Replace("{NAMESPACE}", base.NameSpace);
            if( string.IsNullOrEmpty(textToAppend) == false && textToAppend.Length > 30 )
            {
                int pos = textToAppend.IndexOf("#region");
                if( pos > 0 )
                {
                    ret = ret.Substring( ret.IndexOf("#region"));
                    ret = ret.Substring( 0, ret.IndexOf("#endregion CRUD") + 15);
                    ret = ret + Environment.NewLine + Environment.NewLine + Environment.NewLine + "\t\t";

                    string tmp = textToAppend.Insert( pos, ret);
                    ret = tmp;
                }
            }
            return ret;
        }

        public string FileName
        {
            get { return _fileName; }
        }
    }
}
