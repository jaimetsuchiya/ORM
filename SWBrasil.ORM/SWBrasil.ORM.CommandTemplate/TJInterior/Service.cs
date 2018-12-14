using SWBrasil.ORM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWBrasil.ORM.CommandTemplate.TJInterior
{
    public class Service : CommandBase, ITableTransformation
    {
        public string CommandID { get { return "ServicesSGDAI"; } }
        public string Description { get { return "Gera as classes de serviço no padrão SGDAI!"; } }
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
            string entityName = table.Name.Replace("EFTJ", "");
            _fileName = entityName + "Service";

            var workingColumns = table.Columns;
            if (table.Columns.Where(f => created.Contains(f.ColumnName.ToLower()) || changed.Contains(f.ColumnName.ToLower())).Count() > 0)
            {
                workingColumns.Add(new ColumnModel()
                {
                    ColumnName = "UserCode",
                    DataType = "string",
                    DbType = "nvarchar",
                    Size = 40,
                    DefaultValue = ""
                });
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\tpublic interface I{entityName}Service");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tbool Inserir(IDatabaseCommandCommit databaseCommandCommit, {table.Name} {entityName.ToLower()});");
            sb.AppendLine($"\t\tbool Atualizar(IDatabaseCommandCommit databaseCommandCommit, {table.Name} {entityName.ToLower()});");
            sb.AppendLine($"\t\t{table.Name} Get{entityName}({table.Name} {entityName.ToLower()});");
            sb.AppendLine($"\t\tICollection <{table.Name}> GetAll{entityName}s({table.Name} {entityName.ToLower()});");
            sb.AppendLine("\t}");
            sb.AppendLine("");

            sb.AppendLine($"\tpublic class {entityName}Service : DatabaseCommand<{table.Name}>, I{entityName}Service");
            sb.AppendLine("\t{");
            sb.AppendLine($"private readonly I{entityName}Repository {entityName}Repository;");
            sb.AppendLine("private readonly IDatabaseCommandCommit databaseCommandCommit;");

            sb.AppendLine($"\t\tpublic {entityName}Service(");
            sb.AppendLine($"\t\t\tI{entityName}Repository _{entityName}Repository,");
            sb.AppendLine($"\t\t\tIDatabaseCommandCommit _databaseCommandCommit");
            sb.AppendLine($"\t\t)");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tthis.{entityName}Repository = _{entityName}Repository;");
            sb.AppendLine($"\t\t\tthis.databaseCommandCommit = databaseCommandCommit;");
            sb.AppendLine("\t\t}");

            sb.AppendLine($"\t\tpublic bool Inserir(IDatabaseCommandCommit databaseCommandCommit, {table.Name} {entityName.ToLower()});");
            sb.AppendLine("\t\t{");
            sb.AppendLine(serviceMethod(entityName, "Inserir", "this.databaseCommandCommit"));
            sb.AppendLine("\t\t}");
            sb.AppendLine($"\t\tpublic bool Atualizar(IDatabaseCommandCommit databaseCommandCommit, {table.Name} {entityName.ToLower()});");
            sb.AppendLine("\t\t{");
            sb.AppendLine(serviceMethod(entityName, "Alterar", "this.databaseCommandCommit"));
            sb.AppendLine("\t\t}");
            sb.AppendLine($"\t\tpublic {table.Name} Get{entityName}({table.Name} {entityName.ToLower()});");
            sb.AppendLine("\t\t{");
            sb.AppendLine(serviceMethod(entityName, $"Get{entityName}"));
            sb.AppendLine("\t\t}");
            sb.AppendLine($"\t\tpublic ICollection<{table.Name}> GetAll{entityName}s({table.Name} {entityName.ToLower()});");
            sb.AppendLine("\t\t{");
            sb.AppendLine(serviceMethod(entityName, "GetAll"));
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");

            return sb.ToString();
        }

        private string serviceMethod(string entityName, string method, string databaseCommandCommit = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\treturn this.{entityName}Repository.{method}({(databaseCommandCommit == null ? "" : "databaseCommandCommit, ")}{entityName.ToLower()});");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception ex)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tthrow new Exception(ex.Message);");
            sb.AppendLine("\t\t\t}");

            return sb.ToString();
        }
    }

}
