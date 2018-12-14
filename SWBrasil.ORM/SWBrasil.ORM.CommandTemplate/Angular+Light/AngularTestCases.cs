using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class AngularTestCases : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "AngularTestCases"; }
        }

        public string Description
        {
            get { return "Cria Classe de Testes baseado na implementação Light"; }
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

            string controller = table.Alias.Replace("DTO", "") + "Test";
            _fileName = controller;

            StringBuilder classCode = new StringBuilder();
            if (table.MainDTO == false)
                return "";

            classCode.AppendLine("using System;");
            classCode.AppendLine("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            classCode.AppendLine("using Rhino.Mocks;");
            classCode.AppendLine("using Light;");
            classCode.AppendLine(string.Format("using {0}.Data.Models;", this.NameSpace));
            classCode.AppendLine(string.Format("using {0}.Common.Business;", this.NameSpace));
            classCode.AppendLine(string.Format("using {0}.Common.DTOs;", this.NameSpace));
            classCode.AppendLine(string.Format("using {0}.Core.Business;", this.NameSpace));
            classCode.AppendLine("using SwServices;");
            classCode.AppendLine("using System.Collections.Generic;");
            classCode.AppendLine("");
            classCode.AppendLine("namespace " + this.NameSpace + ".Core.Tests");
            classCode.AppendLine("{");
            classCode.AppendLine("\t[TestClass]");
            classCode.AppendLine("\tpublic class " + controller + "");
            classCode.AppendLine("\t{");
            classCode.AppendLine("\t");

            classCode.AppendLine("\t\t[TestMethod]");
            classCode.AppendLine("\t\t[ExpectedException(typeof(ArgumentNullException))]");
            classCode.AppendLine("\t\tpublic void TestConstructorMethod1()");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t");
            classCode.AppendLine("\t\t\tvar dalArquivo = MockRepository.GenerateMock<IDAL<ArquivoModel>>();");
            classCode.AppendLine("\t\t\tvar iFileType = MockRepository.GenerateStub<IFileType>();");
            classCode.AppendLine("\t\t\t");
            classCode.AppendLine("\t\t\tvar fileBase = new File(null, null);");
            classCode.AppendLine("\t\t\tvar result = fileBase.Get(0);");
            classCode.AppendLine("\t\t");
            classCode.AppendLine("\t\t}");

            classCode.AppendLine("\t}");
            classCode.AppendLine("}");
            classCode.AppendLine("");
            classCode.AppendLine("");

            return classCode.ToString();
        }
    }
}
