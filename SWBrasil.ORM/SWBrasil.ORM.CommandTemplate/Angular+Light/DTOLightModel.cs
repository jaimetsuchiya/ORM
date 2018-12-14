using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class DTOLightModel : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "DTOLightModel "; }
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
            _messages.Add(new ProjectConsoleMessages() { erro = false, data = DateTime.Now, mensagem = string.Format("{0} - Processando Tabela [{1}]", this.CommandID, table.Name) });

            if (table.IgnoreDTO)
                return "";

            string tableName = table.Alias;
            _fileName = tableName;

            StringBuilder classCode = new StringBuilder();
            classCode.AppendLine("using System;");
            classCode.AppendLine("using System.Collections.Generic;");
            classCode.AppendLine("using System.Data;");
            classCode.AppendLine("using System.Linq;");
            classCode.AppendLine("using SwServices.Common;");
            classCode.AppendLine("using System.ComponentModel.DataAnnotations;");
            classCode.AppendLine("using System.Text;");
            classCode.AppendLine("");
            classCode.AppendLine("");
            classCode.AppendLine("namespace " + base.NameSpace + ".Common.DTOs");
            classCode.AppendLine("{");
            classCode.AppendLine(string.Format("\t[BusinessClassMapper(\"{0}\")]", table.ModelName));
            classCode.AppendLine("\t[Serializable()]");
            classCode.AppendLine(string.Format("\tpublic class {0}", table.Alias));
            classCode.AppendLine("\t{");

            string columns = "";
            foreach (ColumnModel col in table.Columns.Where(c=>c.IgnoreOnDTO == false).ToList())
            {
                columns += (columns == "" ? "" : Environment.NewLine);
                //columns += col.Required && col.IsIdentity == false ? ("\t\t[Required]" + Environment.NewLine) : "";
                columns += "\t\t[Display(Name=\"" + col.Label + "\")]" + Environment.NewLine;

                if (string.IsNullOrEmpty(col.RelatedTable) == false && col.UseAsRelatedObject)
                {
                    ColumnModel rtpk = RelatedColumn(col, tables);
                    TableModel rtTB  = RelatedTable(col, tables);
                    if( rtpk != null )
                        columns += "\t\t[BusinessInsidePropertyMapper(\"" + col.ColumnName + "\", \"" + rtpk.DTOName + "\")]" + Environment.NewLine;

                    if( rtTB != null)
                        columns += "\t\tpublic " + rtTB.Alias + " " + rtTB.Alias.Replace("DTO","") + " { get; set; }" + Environment.NewLine;
                }
                else
                {
                    columns += "\t\t[BusinessPropertyMapper(\"" + col.ColumnName + "\")]" + Environment.NewLine;
                    columns += string.Format("\t\tpublic {0} {1}", col.DataType, col.DTOName) + " { get; set; }" + Environment.NewLine;
                }
                columns += Environment.NewLine;
            }

            classCode.Append(columns);
            classCode.AppendLine("");

            //TODO: Adicionar os objetos de associacao
            var tbls = MainTableRelations(table, tables);
            if ( tbls.Count() > 0)
            {
                for( var i=0; i < tbls.Count(); i++)
                {
                    classCode.AppendLine(string.Format("\t\t// Tabela Associada [{0}]", tbls[i].Name));
                    if( tbls[i].Type == enumTableType.Dictionary)
                    {
                        var keyColumn = tbls[i].Columns.Where(c => c.IgnoreOnDTO == false && c.IsUniqueKey).SingleOrDefault();
                        if( keyColumn == null )
                        {
                            _messages.Add(new ProjectConsoleMessages() { erro = true, data = DateTime.Now, mensagem = string.Format("Key Column not found on Dicionary Table [{0}]", tbls[i].Name)});
                            continue;
                        }
                        var valColumn = tbls[i].Columns.Where(c => c.IgnoreOnDTO == false && c.IsUniqueKey == false && c.ColumnName != keyColumn.ColumnName).SingleOrDefault();
                        if (valColumn == null)
                        {
                            _messages.Add(new ProjectConsoleMessages() { erro = true, data = DateTime.Now, mensagem = string.Format("Value Column not found on Dicionary Table [{0}]", tbls[i].Name) });
                            continue;
                        }
                        classCode.AppendLine( "\t\tpublic IDictionary<" + keyColumn.DataType + ", " + valColumn.DataType + "> " + tbls[i].Alias.Replace("DTO", "") + "s { get; set; }" );
                    }
                    else
                    {
                        var cols = tbls[i].Columns.Where(c => c.IgnoreOnDTO == false).ToList();
                        if( cols.Count == 1 )
                            classCode.AppendLine("\t\tpublic List<" + cols.First().DataType + "> " + tbls[i].Alias.Replace("DTO", "") + "s { get; set; }");
                        else if (cols.Count > 1)
                            classCode.AppendLine("\t\tpublic List<" + tbls[i].Alias + "> " + tbls[i].Alias.Replace("DTO", "") + "s { get; set; }");
                        else
                            _messages.Add(new ProjectConsoleMessages() { erro = true, data = DateTime.Now, mensagem = string.Format("DTO [{0}] doesn't have properties", tbls[i].Alias) });
                    }
                    classCode.AppendLine("");
                }
            }
            classCode.AppendLine("\t}");
            classCode.AppendLine("");
            classCode.AppendLine("");
            classCode.AppendLine("");

            classCode.AppendLine(string.Format("\tpublic class Criteria{0}", table.Alias));
            classCode.AppendLine("\t{");

            columns = "";
            foreach (ColumnModel col in table.Columns.Where(c => c.UseAsSearchParameter).ToList())
            {
                columns += (columns == "" ? "" : Environment.NewLine);
                switch( col.DataType )
                {
                    case "int":
                    case "long":
                    case "Guid":
                    case "decimal":
                    case "bool":
                        col.DataType += "?";
                        columns += string.Format("\t\tpublic {0} {1}", col.DataType, col.DTOName) + " { get; set; }" + Environment.NewLine;
                        break;

                    case "DateTime":
                        col.DataType += "?";
                        columns += string.Format("\t\tpublic {0} {1}", col.DataType, col.DTOName + "Start") + " { get; set; }" + Environment.NewLine;
                        columns += string.Format("\t\tpublic {0} {1}", col.DataType, col.DTOName + "Finish") + " { get; set; }" + Environment.NewLine;
                        break;

                    default:
                        columns += string.Format("\t\tpublic {0} {1}", col.DataType, col.DTOName) + " { get; set; }" + Environment.NewLine;
                        break;
                }
                columns += Environment.NewLine;
            }

            classCode.Append(columns);
            classCode.AppendLine("");

            classCode.AppendLine("\t}");
            classCode.AppendLine("");
            classCode.AppendLine("}");

            return classCode.ToString();
        }
    }
}
