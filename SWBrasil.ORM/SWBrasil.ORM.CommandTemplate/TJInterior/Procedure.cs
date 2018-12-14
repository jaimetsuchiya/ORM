using SWBrasil.ORM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWBrasil.ORM.CommandTemplate.TJInterior
{
    public class Procedure : CommandBase, ITableTransformation
    {
        public string CommandID { get { return "ProceduresSGDAI"; } }
        public string Description { get { return "Gera as Procedures no padrão SGDAI!"; } }
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
            string entryParamFields = "@veParametro int";
            string workParamFields = "@vParametro int = @veParametro" + Environment.NewLine;
           
            _fileName = table.Name.ToUpper().Replace("EFTJ", "").ToLower() + "_sgdai";

            var workingColumns = new List<ColumnModel>();
            workingColumns.AddRange(table.Columns);

            var nonIdentityColumns = table.Columns.Where(f => f.IsIdentity == false).OrderBy(f => f.Position).ToList();
            if ( table.Columns.Where(f => created.Contains(f.ColumnName.ToLower()) || changed.Contains(f.ColumnName.ToLower()) ).Count() > 0) {

                workingColumns.Add(new ColumnModel()
                {
                    ColumnName = "UserCode",
                    DataType = "string",
                    DbType = "nvarchar",
                    Size = 40,
                    DefaultValue = ""
                });
            }
            
            foreach (ColumnModel col in workingColumns.Where(f=>created.Contains(f.ColumnName.ToLower()) == false && changed.Contains(f.ColumnName.ToLower()) == false).ToList())
            {
                entryParamFields += (entryParamFields != "" ? "," : "");
                entryParamFields += "@ve" + col.ColumnName + " \t" + col.DbType + (col.Size.HasValue ? "(" + col.Size.ToString() + ")" : "");

                //if (col.Required == false)
                entryParamFields += " = null";

                workParamFields += (workParamFields != "" ? "," : "");
                workParamFields += "@v" + col.ColumnName + " \t" + col.DbType + (col.Size.HasValue ? "(" + col.Size.ToString() + ")" : "") + "= @ve" + col.ColumnName +Environment.NewLine;
            }
            string columns = "";
            string from = "";
            int deep = 1;
            buildFrom(table, tables, ref from, ref deep);

            deep = 0;
            buildSelect(table, tables, "", ref columns, deep);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"IF EXISTS ( SELECT 1 FROM sys.objects WHERE type='P' AND name='{_fileName}')");
            sb.AppendLine($"\tDROP PROC {_fileName}");
            sb.AppendLine("GO");
            sb.AppendLine("");
            sb.AppendLine($"CREATE PROC dbo.{_fileName} (");
            sb.AppendLine(entryParamFields);
            sb.AppendLine(") WITH RECOMPILE AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("");
            sb.AppendLine("\tDECLARE @vErrorMessage nvarchar(max)");
            sb.AppendLine("\tDECLARE " + workParamFields);
            sb.AppendLine("\tBEGIN TRY");

            #region Parametro 1 - Query
            sb.AppendLine("\t\t-- Consulta Lista");
            sb.AppendLine("\t\tIF (@vParametro = 1)");
            sb.AppendLine("\t\tBEGIN");

            sb.AppendLine($"\t\t\tSELECT");
            sb.AppendLine($"\t\t\t{columns}");
            sb.AppendLine($"\t\t\t{from}");
            sb.AppendLine(buildWhereForQuery(table, tables));
            sb.AppendLine("\t\tEND");

            #endregion Parametro 1 - Query

            #region Parametro 2 - Insert
            sb.AppendLine("\t\t-- Inclusão");
            sb.AppendLine("\t\tIF (@vParametro = 2)");
            sb.AppendLine("\t\tBEGIN");

            sb.AppendLine("\t\t\tINSERT");
            sb.AppendLine("\t\t\t\tINTO " + table.Name);
            sb.AppendLine("\t\t\t\t(");
            foreach (ColumnModel col in nonIdentityColumns)
            {
                if(changed.Contains(col.ColumnName.ToLower()) == false)
                    sb.AppendLine("\t\t\t\t" + col.ColumnName);
            }
            sb.AppendLine("\t\t\t\t)");
            sb.AppendLine("\t\t\t\tVALUES");
            sb.AppendLine("\t\t\t\t(");
            for(var c = 0; c < nonIdentityColumns.Count; c++)
            {
                if (changed.Contains(nonIdentityColumns[c].ColumnName.ToLower()) == false)
                {
                    if (c > 0)
                        sb.Append("\t\t\t\t,");

                    if (nonIdentityColumns[c].ColumnName.ToLower() == "createdtime")
                        sb.AppendLine("\t\t\t\tGETDATE()");
                    else if (nonIdentityColumns[c].ColumnName.ToLower() == "createusercode")
                        sb.AppendLine("\t\t\t\t@vUserCode");
                    else
                        sb.AppendLine("\t\t\t\t@v" + nonIdentityColumns[c].ColumnName);
                }
            }
            sb.AppendLine("\t\t\t\t)");

            ColumnModel identityColumn = table.Columns.Where(c => c.IsIdentity == true).SingleOrDefault();
            if (identityColumn != null)
                sb.AppendLine("\t\t\tSELECT SCOPE_IDENTITY() AS " + identityColumn.ColumnName);

            sb.AppendLine("\t\tEND");
            #endregion Parametro 2 - Insert

            #region Parametro 3 - Update
            sb.AppendLine("\t\t-- Alteração");
            sb.AppendLine("\t\tIF (@vParametro = 3)");
            sb.AppendLine("\t\tBEGIN");

            sb.AppendLine("\t\t\tUPDATE");
            sb.AppendLine("\t\t\t\t " + table.Name);
            sb.AppendLine("\t\t\t\tSET");
            for (var c = 0; c < nonIdentityColumns.Count; c++)
            {
                if(created.Contains(nonIdentityColumns[c].ColumnName.ToLower()) == false)
                {
                    if (c > 0)
                        sb.Append("\t\t\t\t,");

                    if (nonIdentityColumns[c].ColumnName.ToLower() == "updtdtime")
                        sb.AppendLine("\t\t\t\t" + nonIdentityColumns[c].ColumnName + " = GETDATE()");
                    else if (nonIdentityColumns[c].ColumnName.ToLower() == "updtusercode")
                        sb.AppendLine("\t\t\t\t" + nonIdentityColumns[c].ColumnName + " = @vUserCode");
                    else
                        sb.AppendLine("\t\t\t\t" + nonIdentityColumns[c].ColumnName + " = @v" + nonIdentityColumns[c].ColumnName);
                }
            }
            sb.AppendLine("\t\t\t\tWHERE");
            sb.AppendLine(buildWhereForSingleRecord(table, tables));

            sb.AppendLine("\t\tEND");
            #endregion Parametro 3 - Update

            #region Parametro 4 - Single
            sb.AppendLine("\t\t-- Consulta Registro");
            sb.AppendLine("\t\tIF (@vParametro = 4)");
            sb.AppendLine("\t\tBEGIN");

            sb.AppendLine($"\t\t\tSELECT");
            sb.AppendLine($"\t\t\t{columns}");
            sb.AppendLine($"\t\t\t{from}");
            sb.AppendLine(buildWhereForSingleRecord(table, tables));

            sb.AppendLine("\t\tEND");

            #endregion Parametro 4 - Single

            sb.AppendLine("\tEND TRY");
            sb.AppendLine("\tBEGIN CATCH");
            sb.AppendLine("\t\tset @vErrorMessage = ERROR_MESSAGE()");
            sb.AppendLine("\t\traiserror(@vErrorMessage, 16, 1)");
            sb.AppendLine("\tEND CATCH");
            sb.AppendLine("GO");

            return sb.ToString();
        }

        private string buildWhereForSingleRecord(TableModel table, List<TableModel> tables)
        {
            string where = "";
            ColumnModel identityColumn = table.Columns.Where(c => c.IsIdentity == true).SingleOrDefault();
            if( identityColumn != null )
            {
                return "\t\t\t\t" + identityColumn.ColumnName + " = @v" + identityColumn.ColumnName;
            }

            List<ColumnModel> pkColumns = table.Columns.Where(c => c.IsPK).ToList();
            if( pkColumns != null && pkColumns.Count > 0 )
            {
                for (var i = 0; i < pkColumns.Count; i++)
                {
                    where += "\t\t\t";
                    where += (i > 0) ? " AND " : "\t";
                    where += pkColumns[i].ColumnName + " = @v" + pkColumns[i].ColumnName;
                    where += Environment.NewLine;
                }
                return where;
            }

            return "";
        }

        private void buildFrom(TableModel table, List<TableModel> tables, ref string from, ref int deep)
        {
            if (string.IsNullOrEmpty(from))
                from = " From " + table.Name + " with(nolock)";

            deep++;
            foreach (ColumnModel col in table.Columns)
            {
                if (string.IsNullOrEmpty(col.RelatedTable) == false)
                {
                    if (created.Contains(col.ColumnName.ToLower()) == false && changed.Contains(col.ColumnName.ToLower()) == false)
                    { 
                        TableModel rTable = base.RelatedTable(col, tables);
                        ColumnModel index = rTable.Columns.Where(c => c.IsPK).FirstOrDefault();
                        if (index != null)
                        {
                            var alias = col.RelatedTable;
                            if (col.ColumnName.ToLower() == "updtusercode")
                                alias = "updated";
                            else if (col.ColumnName.ToLower() == "createusercode")
                                alias = "created";

                            from += "\t\t\t\tLEFT JOIN " + col.RelatedTable + " " + alias + " with(nolock)" + Environment.NewLine;
                            from += "\t\t\t\tON " + table.Name + "." + col.ColumnName + " = " + alias + "." + index.ColumnName + Environment.NewLine;
                            from += Environment.NewLine;
                        }
                        if (deep > 2)
                            return;

                        buildFrom(rTable, tables, ref from, ref deep);
                    }
                }
            }
        }

        private void buildSelect(TableModel table, List<TableModel> tables, string prefix, ref string columns, int deep)
        {
            deep++;

            foreach (ColumnModel col in table.Columns.OrderBy(c=>c.Position).OrderBy(c=>c.RelatedTable))
            {
                var strSufix = "";
                if (string.IsNullOrEmpty(prefix) == false)
                    strSufix = $" as [{prefix}.{col.ColumnName}]";

                var strPrefix = "";
                if (string.IsNullOrEmpty(prefix) == false)
                    strPrefix = $"{prefix}.";

                if (string.IsNullOrEmpty(col.RelatedTable) == false)
                {
                    if ( created.Contains(col.ColumnName.ToLower()) == false && changed.Contains(col.ColumnName.ToLower()) == false )
                    {
                        TableModel rTable = base.RelatedTable(col, tables);
                        if (deep > 2)
                            return;

                        buildSelect(rTable, tables, rTable.Name.Replace("EFTJ", ""), ref columns, deep);
                    }
                    else
                    {
                        if(deep == 1 )
                        {
                            columns += string.IsNullOrEmpty(columns) ? " " : ",";
                            if (col.ColumnName.ToLower() == "createusercode")
                                columns += $"dbo.retorna_usuario_sgdai({strPrefix}CreateUserCode, null) AS CreateUserCode" + Environment.NewLine;
                            else
                                columns += $"dbo.retorna_usuario_sgdai({strPrefix}UpdtUserCode, null) AS UpdtUserCode" + Environment.NewLine;
                        }
                    }
                }
                else
                {
                    
                    if (created.Contains(col.ColumnName.ToLower()) || changed.Contains(col.ColumnName.ToLower()))
                    {
                        if( deep == 1)
                        {
                            columns += string.IsNullOrEmpty(columns) ? " " : ",";
                            columns += strPrefix + col.ColumnName + strSufix + Environment.NewLine;
                        }
                    }
                    else
                    {
                        columns += string.IsNullOrEmpty(columns) ? " " : ",";
                        columns += strPrefix + col.ColumnName + strSufix + Environment.NewLine;
                    }
                }
            }
        }

        private string buildWhereForQuery(TableModel table, List<TableModel> tables)
        {
            string result = "";
            var columns = table.Columns.Where(c => created.Contains(c.ColumnName.ToLower()) == false && changed.Contains(c.ColumnName.ToLower()) == false).ToList();
            for (var i = 0; i < columns.Count; i++)
            {
                result += result == "" ? "WHERE" : "";
                result += "\t\t\t";
                result += i > 0 ? " AND " : "\t";
                result += "(@v" + columns[i].ColumnName + " IS NULL OR ";

                switch (columns[i].DataType)
                {
                    case "string":
                        result += columns[i].ColumnName + " LIKE '%' + @v" + columns[i].ColumnName + " + '%'";
                        break;

                    default:
                        result += columns[i].ColumnName + " = @v" + columns[i].ColumnName;
                        break;
                }
                result += ")" + Environment.NewLine;
            }
            return result;
        }
    }
}
