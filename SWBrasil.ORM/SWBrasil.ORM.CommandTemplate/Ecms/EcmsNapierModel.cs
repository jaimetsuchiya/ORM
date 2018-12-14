using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class EcmsNapierModel : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "EcmsNapierModel"; }
        }

        public string Description
        {
            get { return "Cria Model´s de Persistência no Padrão Napier Ecms"; }
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
            _fileName = table.ModelName;

            StringBuilder classCode = new StringBuilder();
            classCode.AppendLine("using System;");
            classCode.AppendLine("using System.Collections.Generic;");
            classCode.AppendLine("using System.Linq;");
            classCode.AppendLine("using System.Text;");
            classCode.AppendLine("using System.Threading.Tasks;");
            classCode.AppendLine("using WebStore.Common.Service.Models;");
            classCode.AppendLine("");

            classCode.AppendLine("namespace WebStore.Sites.Core");
            classCode.AppendLine("{");
            classCode.AppendLine("");

            classCode.AppendLine("\tpublic class " + table.ModelName + " : Data."  + table.ModelName.Replace("Model", ""));
            classCode.AppendLine("\t{");
            classCode.AppendLine("");
            classCode.AppendLine("\t\t#region Retrieve Methods");
            classCode.AppendLine("");

            var pkColumns = table.Columns.Where(c => c.IsPK).ToList();
            string parameters = "";
            string where = "";
            for (var i = 0; i < pkColumns.Count; i++)
            {
                parameters += parameters == "" ? "" : ", ";
                parameters += pkColumns[i].DataType.Replace("?", "") + " " + DeCapitalize( pkColumns[i].ColumnName );

                where += where == "" ? "" : " AND ";
                switch (pkColumns[i].DataType.Replace("?", ""))
                {
                    case "bool":
                        where += pkColumns[i].ColumnName + "=\" + (" + DeCapitalize(pkColumns[i].ColumnName) + " ? \"1\":\"0\")";
                        break;

                    case "int":
                    case "long":
                    case "decimal":
                        where += pkColumns[i].ColumnName + "=\" + " + DeCapitalize(pkColumns[i].ColumnName) + ".ToString()";
                        break;

                    case "Guid":
                        where += pkColumns[i].ColumnName + "=\" + " + DeCapitalize(pkColumns[i].ColumnName) + ".ToString(\"D\")";
                        break;

                    case "DateTime":
                        where += pkColumns[i].ColumnName + "=\" + " + DeCapitalize(pkColumns[i].ColumnName) + ".ToString(\"yyyy-MM-dd HH:mm:ss\")";
                        break;

                    default:
                        where += pkColumns[i].ColumnName + "='\" + " + DeCapitalize(pkColumns[i].ColumnName) + " + \"'";
                        break;
                }
            }

            classCode.AppendLine("\t\tpublic " + table.ModelName + " ObterPK(" + parameters + ")");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t\treturn " + table.ModelName + ".Find<" + table.ModelName + ">(\"SEL\", \"" + where + ");");
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("");

            pkColumns = table.Columns.Where(c => c.IsUniqueKey).ToList();
            if (pkColumns.Count > 0)
            {
                parameters = "";
                where = "";
                for (var i = 0; i < pkColumns.Count; i++)
                {
                    parameters += parameters == "" ? "" : ", ";
                    parameters += pkColumns[i].DataType.Replace("?", "") + " " + pkColumns[i].ColumnName.ToLowerInvariant();

                    where += where == "" ? "" : " AND ";
                    switch( pkColumns[i].DataType.Replace("?", ""))
                    {
                        case "bool":
                            where += pkColumns[i].ColumnName + "=\" + (" + pkColumns[i].ColumnName.ToLowerInvariant() + " ? \"1\":\"0\");";
                            break;

                        case "int":
                        case "long":
                        case "decimal":
                            where += pkColumns[i].ColumnName + "=\" + " + pkColumns[i].ColumnName.ToLowerInvariant() + ".ToString();";
                            break;

                        case "Guid":
                            where += pkColumns[i].ColumnName + "=\" + " + pkColumns[i].ColumnName.ToLowerInvariant() + ".ToString(\"D\");";
                            break;

                        case "DateTime":
                            where += pkColumns[i].ColumnName + "=\" + " + pkColumns[i].ColumnName.ToLowerInvariant() + ".ToString(\"yyyy-MM-dd HH:mm:ss\");";
                            break;

                        default:
                            where += pkColumns[i].ColumnName + "='\" + " + pkColumns[i].ColumnName.ToLowerInvariant() + " + \"'";
                            break;
                    }
                    
                }
                classCode.AppendLine("\t\tpublic " + table.ModelName + " Obter(" + parameters + ")");
                classCode.AppendLine("\t\t{");
                classCode.AppendLine("\t\t\treturn " + table.ModelName + ".Find<" + table.ModelName + ">(\"SEL\", \"" + where + "\");");
                classCode.AppendLine("\t\t}");
                classCode.AppendLine("");
            }

            classCode.AppendLine("\t\tpublic List<" + table.ModelName + "> Consulta(Consulta" + table.ModelName + " parametros)");
            classCode.AppendLine("\t\t{");
            pkColumns = table.Columns.Where(c => c.IsPK).ToList();
            classCode.AppendLine("\t\t\t#region query");
            classCode.AppendLine("\t\t\tstring query = \"" + pkColumns.First().ColumnName + " is not null;\"");
            foreach (ColumnModel col in table.Columns)
            {
                switch (col.DataType.Replace("?", ""))
                {
                    case "bool":
                        classCode.AppendLine(string.Format("\t\t\tif( parametros.{0}.HasValue )", col.ColumnName));
                        classCode.AppendLine(string.Format("\t\t\t\tquery+= \" AND {0} = \" + parametros.{1}.Value ? \"1\" : \"0\";", col.ColumnName, col.ColumnName));
                        break;

                    case "int":
                    case "long":
                    case "decimal":
                        classCode.AppendLine(string.Format("\t\t\tif( parametros.{0}.HasValue )", col.ColumnName));
                        classCode.AppendLine(string.Format("\t\t\t\tquery+= \" AND {0} = \" + parametros.{1}.Value.ToString();", col.ColumnName, col.ColumnName));
                        classCode.AppendLine("");
                        break;

                    case "Guid":
                        classCode.AppendLine(string.Format("\t\t\tif( parametros.{0}.HasValue )", col.ColumnName));
                        classCode.AppendLine(string.Format("\t\t\t\tquery+= \" AND {0} = '\" + parametros.{1}.Value.ToString(\"D\") + \"'\";", col.ColumnName, col.ColumnName));
                        classCode.AppendLine("");
                        break;

                    case "DateTime":
                        classCode.AppendLine(string.Format("\t\t\tif( parametros.{0}.HasValue )", col.ColumnName));
                        classCode.AppendLine(string.Format("\t\t\t\tquery+= \" AND {0} >= '\" + parametros.{1}.Value.ToString(\"yyyy-MM-dd HH:mm:ss\") + \"'\";", col.ColumnName, col.ColumnName));
                        classCode.AppendLine("");

                        classCode.AppendLine(string.Format("\t\t\tif( parametros.{0}.HasValue )", col.ColumnName));
                        classCode.AppendLine(string.Format("\t\t\t\tquery+= \" AND {0} <= '\" + parametros.{1}.Value.ToString(\"yyyy-MM-dd HH:mm:ss\") + \"'\";", col.ColumnName, col.ColumnName));
                        classCode.AppendLine("");
                        break;
                    default:
                        classCode.AppendLine(string.Format("\t\t\tif(string.IsNullOrEmpty(parametros.{0}) == false )", col.ColumnName));
                        classCode.AppendLine(string.Format("\t\t\t\tquery+= \" AND {0} LIKE '%\" + parametros.{1}+ \"'%\";", col.ColumnName, col.ColumnName));
                        classCode.AppendLine("");
                        break;
                }
            }

            classCode.AppendLine("\t\t\t#endregion query");
            classCode.AppendLine("");
            classCode.AppendLine("\t\t\treturn " + table.ModelName + ".FindAll<" + table.ModelName + ">(\"SEL\", query);");
            classCode.AppendLine("");
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("");
            classCode.AppendLine("\t\t#endregion Retrieve Methods");
            classCode.AppendLine("");
            classCode.AppendLine("");
            classCode.AppendLine("\t\t#region Persistence Methods");
            classCode.AppendLine("");
            classCode.AppendLine("\t\tpublic new int Save()");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t\treturn base.Save();");
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("");
            classCode.AppendLine("\t\tpublic new int Delete()");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t\treturn base.Delete();");
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("\t\t#endregion Persistence Methods");
            classCode.AppendLine("");
            classCode.AppendLine("");
            classCode.AppendLine("\t}");

            classCode.AppendLine("");
            classCode.AppendLine("");
            classCode.AppendLine("\tpublic class Consulta" + table.ModelName);
            classCode.AppendLine("\t{");
            foreach(ColumnModel col in table.Columns)
            {
                string nullable = "";
                switch (col.DataType)
                {
                    case "int":
                    case "long":
                    case "Guid":
                    case "decimal":
                    case "bool":
                        nullable = "?";
                        classCode.AppendLine(string.Format("\t\tpublic {0} {1}", (col.DataType + nullable), col.ColumnName) + " { get; set; }");
                        break;

                    case "DateTime":
                        nullable = "?";
                        classCode.AppendLine(string.Format("\t\tpublic {0} {1}", (col.DataType + nullable), col.ColumnName + "Inicial") + " { get; set; }");
                        classCode.AppendLine(string.Format("\t\tpublic {0} {1}", (col.DataType + nullable), col.ColumnName + "Final") + " { get; set; }");
                        break;

                    default:
                        classCode.AppendLine(string.Format("\t\tpublic {0} {1}", col.DataType, col.ColumnName) + " { get; set; }");
                        break;
                }
            }

            classCode.AppendLine("\t}");
            classCode.AppendLine("");
            classCode.AppendLine("");
            classCode.AppendLine("}");

            return classCode.ToString();
        }
    }
}
