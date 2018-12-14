using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class LightAbstractModel : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "LightAbstractModel"; }
        }

        public string Description
        {
            get { return "Cria Model´s de Persistência no Padrão Light"; }
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

            string propriedades = "";
            string variaveis = "";
            string tableName = table.ModelName.Replace("Model", "BaseModel");
            _fileName = tableName;

            StringBuilder classCode = new StringBuilder();
            classCode.AppendLine("using Light;");
            classCode.AppendLine("using Light.Validation;");
            classCode.AppendLine("using System;");
            classCode.AppendLine("using System.Collections.Generic;");
            classCode.AppendLine("using System.Data;");
            classCode.AppendLine("using System.Linq;");
            classCode.AppendLine("using System.Text;");
            classCode.AppendLine("using System.Threading.Tasks;");
            classCode.AppendLine("");
            classCode.AppendLine("");
            classCode.AppendLine("namespace " + base.NameSpace + ".Data.Models");
            classCode.AppendLine("{");
            if (string.IsNullOrEmpty(this.ConnectionStringID) == false)
                classCode.AppendLine("\t[CustomConnectionStringID(\"" + this.ConnectionStringID + "\")]");

            classCode.AppendLine(string.Format("\t[Table(\"{0}\", \"dbo\")]", table.Name));
            classCode.AppendLine("\t[Serializable()]");
            classCode.AppendLine(string.Format("\tpublic abstract class {0} : Light.EntityBase", tableName));
            classCode.AppendLine("\t{");
            classCode.AppendLine("\t\t#region Members");
            classCode.AppendLine("[VARIAVEIS]");
            classCode.AppendLine("\t\t#endregion");
            classCode.AppendLine("");
            classCode.AppendLine("\t\t#region Properties");
            classCode.AppendLine("[PROPRIEDADES]");
            classCode.AppendLine("\t\t#endregion");
            classCode.AppendLine("");
            classCode.AppendLine("");
            classCode.AppendLine("\t\t#region Business rules");

            classCode.AppendLine("\t\tprotected override void CheckRules()");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t\tbase.CheckRules();");
            classCode.AppendLine("\t\t}");

            classCode.AppendLine("\t\t#endregion");
            classCode.AppendLine("\t}");

            classCode.AppendLine();
            classCode.AppendLine();
            classCode.AppendLine();
            classCode.AppendLine();

            //classCode.AppendLine(string.Format("\tpublic partial class {0}Model : {1}", tableName, (tableName + "BaseModel")));
            //classCode.AppendLine("\t{");

            //if(table.MainDTO)
            //{
            //    var relatedColumns = table.Columns.Where(col => string.IsNullOrEmpty(col.RelatedTable) == false).ToList();
            //    for (var i = 0; i < relatedColumns.Count; i++)
            //    {
            //        var dto = tables.Where(t => t.Name == relatedColumns[i].RelatedTable).SingleOrDefault();
            //        if (dto != null)
            //        {
            //            string objName = (string.IsNullOrEmpty(dto.Alias) ? dto.Name : dto.Alias);
            //            if (objName == dto.Name)
            //                _messages.Add(new ProjectConsoleMessages() { mensagem = string.Format("Tabela [{0}] nao possui o DTO Name parametrizado!", dto.Name), erro = true, data = DateTime.Now });

            //            classCode.AppendLine(string.Format("\t\tpublic List<{0}> { get; set; }", objName));
            //            classCode.AppendLine("");
            //        }
            //    }

            //}
            //classCode.AppendLine("\t}");

            //classCode.AppendLine();
            //classCode.AppendLine();
            //classCode.AppendLine();
            //classCode.AppendLine();

            //classCode.AppendLine(string.Format("\tpublic class {0}Data : DAL<{1}>", tableName, _fileName));
            //classCode.AppendLine("\t{");
            //classCode.AppendLine("\t\tpublic " + tableName + "Data(string connectionStringID) : base(connectionStringID) { }");
            //classCode.AppendLine(string.Format("\t\tpublic List<{0}> List(Query query, int pageSize, int currentPage, out int pageCount)", _fileName));
            //classCode.AppendLine("\t\t{");
            //classCode.AppendLine("\t\t\treturn base.List(query, pageSize, currentPage, out pageCount);");
            //classCode.AppendLine("\t\t}");

            //if (table.MainDTO)
            //{
            //    //TODO: Save All
            //    //TODO: Load All
            //}
            //classCode.AppendLine("\t}");
            classCode.AppendLine("}");


            foreach (ColumnModel col in table.Columns)
            {
                variaveis += (variaveis == "" ? "" : Environment.NewLine);
                string atributo = string.Format("\t\t[Column(\"{0}\", {1}", col.ColumnName, DataTypeToDbType(col.DataType));
                if (col.Size.HasValue)
                    atributo += ", " + col.Size.Value.ToString();

                if( col.IsPK )
                    atributo += ", PrimaryKey = true";

                if( col.IsIdentity )
                    atributo += ", AutoIncrement = true";

                atributo += ")]";

                variaveis += atributo + Environment.NewLine;
                variaveis += "\t\tprivate " + col.DataType + " _" + col.ColumnName + ";" + Environment.NewLine;

                propriedades+= (propriedades == "" ? "" : Environment.NewLine);
                propriedades+= "\t\tpublic " + col.DataType + " " + col.ColumnName + Environment.NewLine;
                propriedades+= "\t\t{" + Environment.NewLine;
                propriedades+= "\t\t\tget { return _" + col.ColumnName + "; }" + Environment.NewLine;
                propriedades+= "\t\t\tset { _" + col.ColumnName + " = value; }" + Environment.NewLine;
                propriedades+= "\t\t}" + Environment.NewLine;
            }


            return classCode.ToString().Replace("[VARIAVEIS]", variaveis).Replace("[PROPRIEDADES]", propriedades);
        }
    }
}
