using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWBrasil.ORM.Common
{
    public interface ICommand
    {
        string CommandID { get; }
        string ConnectionStringID { get; set; }
        string Extension { get; }
        string Description { get; }
        string FileName { get; }

        string Directory { get; }
        string ProjectName { get; set; }
        string NameSpace { get; set; }
        bool useGroupAsFile { get; }

        List<ProjectConsoleMessages> Mensagens { get; }
    }

    public interface ITableTransformation: ICommand
    {
        string ApplyTemplate(TableModel table, List<TableModel> tables = null, string textToAppend = null);
    }

    public interface IProcedureTransformation : ICommand
    {
        string ApplyTemplate(ProcModel procedure);
    }
}
