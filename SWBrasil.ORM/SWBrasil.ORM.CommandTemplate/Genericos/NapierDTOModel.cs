using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class NapierDTOModel : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "NapierDTOModel"; }
        }

        public string Description
        {
            get { return "Cria Model´s de Persistência no Padrão DTO"; }
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
            /*
             * 0 - Table Name
             * 1 - Columns
             
            [MapperAttribute(Name = "ID", IsKey = true, IsIdentity = true)]
            public int IDTabela { get; set; }

            [MapperAttribute(Name = "Nome")]
            public string NomeTeste { get; set; }
            */

            string columns = "";
            _fileName = table.Alias;
            _directoryName = this.ProjectName + ".Common\\DTO\\" + table.Group;

            foreach (ColumnModel col in table.Columns)
            {
                columns += (columns == "" ? "" : Environment.NewLine);
                columns += col.Required && col.IsIdentity == false ? ("\t\t[Required]" + Environment.NewLine) : "";
                
                if (string.IsNullOrEmpty(col.RelatedTable) == false)
                {
                    ColumnModel rtpk = RelatedColumn(col, tables);
                    TableModel rltb = RelatedTable(col, tables);

                    columns += "\t\t[DataRowPrefixPropertyMapper(\"" + col.RelatedTable.Replace("tb_", "") + "_\")]" + Environment.NewLine;
                    columns += "\t\t[NapierInsidePropertyMapper(\"" + col.ColumnName + "\", \"" + rtpk.DTOName + "\")]" + Environment.NewLine;
                    columns += "\t\tpublic " + rltb.Alias + " " + rltb.Alias.Replace("DTO", "") + " { get; set; }" + Environment.NewLine;
                }
                else
                {
                    columns += "\t\t[NapierPropertyMapper(\"" + col.ColumnName + "\")]" + Environment.NewLine;
                    columns += "\t\t[DataRowPropertyMapper(\"" + col.ColumnName + "\")]" + Environment.NewLine;
                    columns += string.Format("\t\tpublic {0} {1}", col.DataType, col.DTOName) + " { get; set; }" + Environment.NewLine;
                }
                columns += Environment.NewLine;
            }

            return Templates.Default.DTOModel.Replace("{0}", _fileName).Replace("{1}", columns).Replace("{2}", table.ModelName).Replace("{3}", base.NameSpace);
        }
    }
}
