using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class SimpleInjector : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "SimpleInjector"; }
        }

        public string Description
        {
            get { return "Cria o Registro das Interfaces no SimpleInjector"; }
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

            _fileName = "MvcApplication";

            StringBuilder classCode = new StringBuilder();
            classCode.AppendLine("\t\tprivate void Register(ref Container container)");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t\tcontainer.Register<SwServices.Common.Business.IApplication, SwServices.Core.Business.Application>(Lifestyle.Scoped);");
            classCode.AppendLine("\t\t\tcontainer.Register<IDAL<SwServices.Data.Models.AplicacaoLogModel>, DAL<SwServices.Data.Models.AplicacaoLogModel>>(Lifestyle.Scoped);");

            foreach (TableModel tbl in tables)
            {
                if (tbl.IgnoreDTO == false)
                    classCode.AppendLine(string.Format("\t\t\tcontainer.Register<I{0}, {0}>(Lifestyle.Scoped);", tbl.Alias.Replace("DTO", "")));

                classCode.AppendLine(string.Format("\t\t\tcontainer.Register<IDAL<{0}>, DAL<{0}>>(Lifestyle.Scoped);", tbl.ModelName));
            }

            classCode.AppendLine("\t\t}");

            return classCode.ToString();
        }
    }
}

