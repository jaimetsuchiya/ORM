using SWBrasil.ORM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWBrasil.ORM.CommandTemplate.TJInterior
{
    public class Repository : CommandBase, ITableTransformation
    {
        public string CommandID { get { return "RepositoriesSGDAI"; } }
        public string Description { get { return "Gera as classes de repositório no padrão SGDAI!"; } }
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
            _fileName = entityName + "Repository";

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
            sb.AppendLine($"\tpublic interface I{entityName}Repository");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tbool Inserir(IDatabaseCommandCommit databaseCommandCommit, {table.Name} {entityName.ToLower()});");
            sb.AppendLine($"\t\tbool Atualizar(IDatabaseCommandCommit databaseCommandCommit, {table.Name} {entityName.ToLower()});");
            sb.AppendLine($"\t\t{table.Name} Get{entityName}({table.Name} {entityName.ToLower()});");
            sb.AppendLine($"\t\tICollection <{table.Name}> GetAll{entityName}s({table.Name} {entityName.ToLower()});");
            sb.AppendLine("\t\t}");
            sb.AppendLine("");

            sb.AppendLine($"\tpublic class {entityName}Repository : DatabaseCommand<{table.Name}>, I{entityName}Repository");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tpublic string Procedure");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tget { return \"" + entityName.ToString() + "_sgdai\"; }");
            sb.AppendLine("\t\t}");

            sb.AppendLine($"\t\tprivate List<SqlParameter> SetProcedureParameters(int parameterId, {table.Name} {entityName})");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tvar parameters = new List<SqlParameter>();");
            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\t\tparameters.Add(new SqlParameter(\"@veParametro\", parameterId));");

            foreach (ColumnModel col in workingColumns.Where(f => created.Contains(f.ColumnName.ToLower()) == false && changed.Contains(f.ColumnName.ToLower()) == false).ToList())
            {
                if(col.Required)
                    sb.AppendLine($"\t\t\t\tparameters.Add(new SqlParameter(\"@ve{col.ColumnName}\", {entityName}.{col.ColumnName}));");
                else
                {
                    switch(col.DbType.Trim().ToUpper())
                    {
                        case "BIGINT":
                        case "INT":
                        case "SMALLINT":
                        case "MONEY":
                        case "DECIMAL":
                            sb.AppendLine($"\t\t\t\tif( {entityName}.{col.ColumnName} > 0 )");
                            sb.AppendLine($"\t\t\t\t\tparameters.Add(new SqlParameter(\"@ve{col.ColumnName}\", {entityName}.{col.ColumnName}));");
                            break;

                        case "DATE":
                        case "DATETIME":
                        case "SMALLDATETIME":
                        case "TIME":
                            sb.AppendLine($"\t\t\t\tif( {entityName}.{col.ColumnName} != DateTime.MinValue )");
                            sb.AppendLine($"\t\t\t\t\tparameters.Add(new SqlParameter(\"@ve{col.ColumnName}\", {entityName}.{col.ColumnName}));");
                            break;

                        default:
                            if(col.ColumnName == "UserCode")
                            {
                                sb.AppendLine($"\t\t\t\tif( string.IsNullOrEmpty({entityName}.UpdtUserCode) == false )");
                                sb.AppendLine($"\t\t\t\t\tparameters.Add(new SqlParameter(\"@ve{col.ColumnName}\", {entityName}.UpdtUserCode));");
                            }
                            else
                            {
                                sb.AppendLine($"\t\t\t\tif( string.IsNullOrEmpty({entityName}.{col.ColumnName}) == false )");
                                sb.AppendLine($"\t\t\t\t\tparameters.Add(new SqlParameter(\"@ve{col.ColumnName}\", {entityName}.{col.ColumnName}));");
                            }
                            break;
                    }
                }
            }

            sb.AppendLine("\t\t\t");
            sb.AppendLine("\t\t\treturn parameters;");
            sb.AppendLine("\t\t}");

            sb.AppendLine($"\t\tpublic bool Inserir(IDatabaseCommandCommit databaseCommandCommit, {table.Name} {entityName.ToLower()});");
            sb.AppendLine("\t\t{");
            sb.AppendLine(serviceMethod(workingColumns, 2, "databaseCommandCommit", "Insert", entityName));
            sb.AppendLine("\t\t}");
            sb.AppendLine($"\t\tpublic bool Atualizar(IDatabaseCommandCommit databaseCommandCommit, {table.Name} {entityName.ToLower()});");
            sb.AppendLine("\t\t{");
            sb.AppendLine(serviceMethod(workingColumns, 3, "databaseCommandCommit", "Update", entityName));
            sb.AppendLine("\t\t}");
            sb.AppendLine($"\t\tpublic {table.Name} Get{entityName}({table.Name} {entityName.ToLower()});");
            sb.AppendLine("\t\t{");
            sb.AppendLine(serviceMethod(workingColumns, 4, "base", "GetEntity", entityName));
            sb.AppendLine("\t\t}");
            sb.AppendLine($"\t\tpublic ICollection<{table.Name}> GetAll{entityName}s({table.Name} {entityName.ToLower()});");
            sb.AppendLine("\t\t{");
            sb.AppendLine(serviceMethod(workingColumns, 1, "base", "Select", entityName));
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
        
            return sb.ToString();
        }

        private string serviceMethod(List<ColumnModel> workingColumns, int parametro, string provider, string method, string entityName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t\t\ttry");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\tList<SqlParameter> parameters = SetProcedureParameters({parametro.ToString()}, {entityName.ToLower()});");
            var identity = workingColumns.Where(c => c.IsIdentity).SingleOrDefault();
            if (identity != null && provider != "base" )
            {
                var prefix = (method == "Inserir" ? "var identity = " : "");
                sb.AppendLine($"\t\t\t\t{prefix}{provider}.{method}(Procedure, parameters);");
                sb.AppendLine($"\t\t\t\treturn true;");
            }
            else
            {
                sb.AppendLine($"\t\t\t\treturn {provider}.{method}(Procedure, parameters);");
            }
                
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\tcatch(Exception ex)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tthrow new Exception(ex.Message);");
            sb.AppendLine("\t\t\t}");

            return sb.ToString();
        }
    }
    
}

