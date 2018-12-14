using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class NapierModel : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "NapierModel"; }
        }

        public string Description
        {
            get { return "Cria Model´s de Persistência no Padrão Napier"; }
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

            /*
             * 0 - Table Name
             * 1 - Columns
             
            [MapperAttribute(Name = "ID", IsKey = true, IsIdentity = true)]
            public int IDTabela { get; set; }

            [MapperAttribute(Name = "Nome")]
            public string NomeTeste { get; set; }
            */
            string className = table.ModelName;
            _fileName = className;

            string columns = "";
            string isPK = "";
            string isIdentity = "";
            string search = "";
            foreach (ColumnModel col in table.Columns)
            {
                isPK = col.IsPK ? ", IsKey = true" : "";
                isIdentity = col.IsIdentity ? ", IsIdentity = true" : "";
                
                columns += (columns == "" ? "" : Environment.NewLine);
                columns += col.Required && col.IsIdentity == false ? ("\t\t[Required]" + Environment.NewLine) : "";
                columns += string.Format("\t\t[MapperAttribute(Name = \"{0}\" {1} {2})]", col.ColumnName, isPK, isIdentity) + Environment.NewLine;
                columns += string.Format("\t\tpublic {0} {1}", col.DataType, col.ColumnName) + " { get; set; }" + Environment.NewLine;
                columns += Environment.NewLine;

            }

            var pkColumns = table.Columns.Where(c => c.IsPK).ToList();
            string parameters = "";
            string where = "";
            for( var i=0; i < pkColumns.Count; i++)
            {
                parameters += parameters == "" ? "" : ", ";
                parameters += pkColumns[i].DataType + " " + pkColumns[i].ColumnName.ToLowerInvariant();

                where += where == "" ? "" : " AND ";
                where += pkColumns[i].ColumnName + "='\" + " + pkColumns[i].ColumnName.ToLowerInvariant() + " + \"'";
            }

            search = "\t\tpublic static " + table.Name.Replace("tb_", "") + "Model" + " FindByPK(" + parameters + ")" + Environment.NewLine;
            search += "\t\t{" + Environment.NewLine;
            search += "\t\t\t" + "return " + table.Name.Replace("tb_", "") + "Model.Find<" + table.Name.Replace("tb_", "") + "Model>(\"SEL\", \"" + where + "\");" + Environment.NewLine;
            search += "\t\t}" + Environment.NewLine;
            search += Environment.NewLine;


            pkColumns = table.Columns.Where(c => c.IsUniqueKey).ToList();
            if( pkColumns.Count > 0 )
            { 
                parameters = "";
                where = "";
                for (var i = 0; i < pkColumns.Count; i++)
                {
                    parameters += parameters == "" ? "" : ", ";
                    parameters += pkColumns[i].DataType + " " + pkColumns[i].ColumnName.ToLowerInvariant();

                    where += where == "" ? "" : " AND ";
                    where += pkColumns[i].ColumnName + "='\" + " + pkColumns[i].ColumnName.ToLowerInvariant() + " + \"'";
                }

                search += "\t\tpublic static " + table.Name.Replace("tb_", "") + "Model" + " FindByUniqueKey(" + parameters + ")" + Environment.NewLine;
                search += "\t\t{" + Environment.NewLine;
                search += "\t\t\t" + "return " + table.Name.Replace("tb_", "") + "Model.Find<" + table.Name.Replace("tb_", "") + "Model>(\"SEL\", \"" + where + "\");" + Environment.NewLine;
                search += "\t\t}" + Environment.NewLine;
                search += Environment.NewLine;
            }


            return Templates.Default.NapierModel.Replace("{0}", table.Name).Replace("{1}", columns).Replace("{2}", search).Replace("{3}", base.NameSpace).Replace("{4}", className);
        }


        //private void buildSearchParameters(TableModel table, List<TableModel> tables, ref string parameters, ref int deep)
        //{
        //    deep++;
        //    foreach (ColumnModel col in table.Columns)
        //    {
        //        if (string.IsNullOrEmpty(col.RelatedTable) == false)
        //        {
        //            TableModel rTable = base.RelatedTable(col, tables);
        //            if (deep > 3)
        //                return;
        //            buildSearchParameters(rTable, tables, ref parameters, ref deep);
        //        }
        //        else
        //        {
        //            switch (col.DataType)
        //            {
        //                case "DateTime":
        //                    parameters += "\t\t\tif(args." + col.ColumnName + "Ini.HasValue)" + Environment.NewLine;
        //                    parameters += "\t\t\t\tcmd.Parameters.Add(new SqlParameter(\"@" + table.Name.Replace("tb_", "") + "_" + col.ColumnName + "Ini\", args." + col.ColumnName + "Ini));" + Environment.NewLine;

        //                    parameters += "\t\t\tif(args." + col.ColumnName + "Fim.HasValue)" + Environment.NewLine;
        //                    parameters += "\t\t\t\tcmd.Parameters.Add(new SqlParameter(\"@" + table.Name.Replace("tb_", "") + "_" + col.ColumnName + "Fim\", args." + col.ColumnName + "Fim));" + Environment.NewLine;
        //                    break;

        //                case "string":
        //                    parameters += "\t\t\tif(string.isNullOrEmpty(args." + col.ColumnName + ") == false)" + Environment.NewLine;
        //                    parameters += "\t\t\t\tcmd.Parameters.Add(new SqlParameter(\"@" + table.Name.Replace("tb_", "") + "_" + col.ColumnName + "\", args." + col.ColumnName + "));" + Environment.NewLine;
        //                    break;

        //                default:
        //                    parameters += "\t\t\tif(args." + col.ColumnName + ".HasValue)" + Environment.NewLine;
        //                    parameters += "\t\t\t\tcmd.Parameters.Add(new SqlParameter(\"@" + table.Name.Replace("tb_", "") + "_" + col.ColumnName + "\", args." + col.ColumnName + "));" + Environment.NewLine;
        //                    break;
        //            }
        //        }
        //    }
        //}
    }
}
