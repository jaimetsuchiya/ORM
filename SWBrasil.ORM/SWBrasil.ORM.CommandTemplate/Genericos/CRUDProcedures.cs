using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class CRUDProcedures : CommandBase, ITableTransformation
    {
        public string CommandID { get { return "CRUDProcedures"; } }
        public string Description { get { return "Gera as Procedures de CRUD para a Tabela Informada!"; } }

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
            string indexFields = "";
            string saveFields = "";
            string whereFields = "";
            string nonIndexFields = "";
            string nonIFValues = "";
            string updFields = "";
            string identity = "";
            string[] created = new string[] { "criadoEm", "criadoPor"};
            string[] changed = new string[] { "alteradoEm", "alteradoPor" };
            _fileName = table.Name.Replace("tb_", "") + "_scripts";

            foreach (ColumnModel col in table.Columns.Where(c => c.IsPK == true).ToList())
            {
                indexFields += (indexFields != "" ? (Environment.NewLine + ",") : "");
                indexFields += "@" + col.ColumnName + " \t" + col.DbType + (col.Size.HasValue ? "(" + col.Size.ToString() + ")" : "");
                if (col.IsIdentity)
                    indexFields += " = null output";

                whereFields += (whereFields != "" ? " AND " : "");
                whereFields += col.ColumnName + " = @" + col.ColumnName;

                nonIndexFields += (nonIndexFields != "" ? ", " : "");
                nonIndexFields += col.ColumnName;

                nonIFValues += (nonIFValues != "" ? ", " : "");
                nonIFValues += "@" + col.ColumnName;
            }
            foreach (ColumnModel col in table.Columns.Where(c => c.IsPK == false).ToList())
            {
                nonIndexFields += (nonIndexFields != "" ? ", " : "");
                nonIndexFields += col.ColumnName;

                nonIFValues += (nonIFValues != "" ? ", " : "");
                nonIFValues += "@" + col.ColumnName;

                if (created.Contains(col.ColumnName) == false)
                {
                    updFields += (updFields != "" ? ", " : "");
                    updFields += col.ColumnName + " = @" + col.ColumnName;
                }

                saveFields += (Environment.NewLine + ",");
                saveFields += "@" + col.ColumnName + " \t" + col.DbType + (col.Size.HasValue ? "(" + col.Size.ToString() + ")" : "");
                if (col.Required == false)
                    saveFields += " = null";

            }
            ColumnModel identityColumn = table.Columns.Where(c => c.IsIdentity == true).SingleOrDefault();
            if (identityColumn != null)
                identity = "SET @" + identityColumn.ColumnName + " = SCOPE_IDENTITY()";

            string deleteCmd = string.Format(Templates.Default.Procedure_Delete, table.Name, whereFields);
            string saveCmd   = string.Format(Templates.Default.Procedure_Save, table.Name, whereFields, updFields, nonIndexFields, nonIFValues, identity);
            string selectCmd = string.Format(Templates.Default.Procedure_Select, nonIndexFields, table.Name, whereFields);

            string  ret = textToAppend + Environment.NewLine;
                    ret+=string.Format(Templates.Default.Procedure_Base, ("pDel" + table.Name.Replace("tb_", "")), indexFields.Replace(" = null output", ""), deleteCmd);
                    ret+= string.Format(Templates.Default.Procedure_Base, ("pSel" + table.Name.Replace("tb_", "")), indexFields.Replace(" = null output", ""), selectCmd);
                    ret+= string.Format(Templates.Default.Procedure_Base, ("pSave" + table.Name.Replace("tb_", "")), (indexFields + saveFields), saveCmd);

            int deep = 0;
            string join = "";
            buildJoin(table, tables, ref join, ref deep);

            deep = 0;
            string columns = "";
            buildSelect(table, tables, "", ref columns, ref deep);

            deep = 0;
            string where = "";
            buildWhere(table, tables, ref where, ref deep);

            deep = 0;
            string parameters = "";
            buildParam(table, tables, ref parameters, ref deep);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("IF EXISTS ( SELECT 1 FROM sys.objects WHERE type='P' AND name='{0}')");
	        sb.AppendLine("\tDROP PROC {0}");
            sb.AppendLine("GO");
            sb.AppendLine("");
            sb.AppendLine("CREATE PROC dbo.{0} (");
            sb.AppendLine(parameters.Substring(1));
            sb.AppendLine(") AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("");
            sb.AppendLine("\tSELECT");
            sb.AppendLine(columns.Substring(1));
            sb.AppendLine("FROM");
            sb.AppendLine(table.Name + " as " + table.Name);
            sb.AppendLine(join);
            sb.AppendLine("WHERE");
            sb.AppendLine(where);
            sb.AppendLine("");
            sb.AppendLine("END");
            sb.AppendLine("GO");
            ret += sb.ToString().Replace("{0}", "p_Search" + table.Name.Replace("tb_", ""));

            return ret;
        }

        private void buildJoin(TableModel table, List<TableModel> tables, ref string join, ref int deep)
        {
            deep ++;
            foreach (ColumnModel col in table.Columns)
            {
                if (string.IsNullOrEmpty(col.RelatedTable) == false)
                {
                    TableModel rTable = base.RelatedTable(col, tables);
                    ColumnModel index = rTable.Columns.Where(c => c.IsPK).FirstOrDefault();
                    if (index != null)
                    {
                        join += "LEFT JOIN " + col.RelatedTable + " " + col.RelatedTable + " (nolock)" + Environment.NewLine;
                        join += "ON " + table.Name + "." + col.ColumnName + " = " + col.RelatedTable + "." + index.ColumnName + Environment.NewLine;
                        join += Environment.NewLine;
                    }
                    if (deep > 3)
                        return;
                    buildJoin(rTable, tables, ref join, ref deep);
                }
            }
        }


        private void buildSelect(TableModel table, List<TableModel> tables, string prefix, ref string columns, ref int deep)
        {
            deep++;
            foreach (ColumnModel col in table.Columns)
            {
                if (string.IsNullOrEmpty(col.RelatedTable) == false)
                {
                    TableModel rTable = base.RelatedTable(col, tables);
                    if (deep > 3)
                        return;
                    buildSelect(rTable, tables, rTable.Name.Replace("tb_", ""), ref columns, ref deep);
                }
                else
                {
                    string tableName = (deep == 1?"": table.Name.Replace("tb_", "") + "_");
                    if (prefix == table.Name.Replace("tb_", ""))
                        prefix = "";

                    columns += table.Name + "." + col.ColumnName + " as " + prefix + ( prefix == "" ? "" : "_") + tableName + col.ColumnName + Environment.NewLine;
                }
            }
        }

        private void buildWhere(TableModel table, List<TableModel> tables, ref string where, ref int deep)
        {
            deep++;
            foreach (ColumnModel col in table.Columns)
            {
                if (string.IsNullOrEmpty(col.RelatedTable) == false)
                {
                    TableModel rTable = base.RelatedTable(col, tables);
                    if (deep > 3)
                        return;
                    buildWhere(rTable, tables, ref where, ref deep);
                }
                else
                {
                    string param = "@" + table.Name.Replace("tb_", "") + "_" + col.ColumnName;
                    where += "AND(" + param + " IS NULL OR ";
                    switch (col.DataType)
                    {
                        case "DateTime":
                            where += table.Name + "." + col.ColumnName + " BETWEEN " + param + "Ini" + " AND " + param + "Fim)" + Environment.NewLine;
                            break;

                        case "string":
                            where += table.Name + "." + col.ColumnName + " LIKE '%' + " + param + " + '%')" + Environment.NewLine;
                            break;

                        default:
                            where += table.Name + "." + col.ColumnName + "=" + param + ")" + Environment.NewLine;
                            break;
                    }
                }
            }
        }

        private void buildParam(TableModel table, List<TableModel> tables, ref string parameters, ref int deep)
        {
            deep++;
            foreach (ColumnModel col in table.Columns)
            {
                if (string.IsNullOrEmpty(col.RelatedTable) == false)
                {
                    TableModel rTable = base.RelatedTable(col, tables);
                    if (deep > 3)
                        return;
                    buildParam(rTable, tables, ref parameters, ref deep);
                }
                else
                {
                    parameters+= ",@" + table.Name.Replace("tb_", "") + "_" + col.ColumnName + "      " + col.DbType;
                    switch (col.DataType)
                    {
                        case "decimal":
                            parameters += "(" + col.Size + "," + col.Precision + ") = NULL";
                            break;

                        case "string":
                            parameters += "(" + col.Size + ") = NULL";
                            break;

                        case "DateTime":
                            parameters += "Ini" + Environment.NewLine + ",@" + table.Name.Replace("tb_", "") + "_" + col.ColumnName + "      " + col.DbType + "Fim = NULL";
                            break;

                        default:
                            parameters += " = NULL";
                            break;
                    }
                    parameters += Environment.NewLine;
                }
            }

        }
    }
}
