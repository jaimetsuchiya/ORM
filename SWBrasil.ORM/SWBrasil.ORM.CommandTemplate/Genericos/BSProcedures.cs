using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class BSProcedures : CommandBase, IProcedureTransformation
    {
        public string CommandID { get { return "BusinessProcedures"; } }
        public string Description { get { return "Gera Interface de Chamada para as Procedures de Business"; } }

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

        public string ApplyTemplate(ProcModel procedure)
        {
            _fileName = procedure.Name;
            StringBuilder ret = new StringBuilder();
            StringBuilder procedureParameters = new StringBuilder(); 
            string[] ignorParameters = new string[]{"@abrirtransacao", "@debug"};
            string methodParameters = "";
            string codigoRetorno = "";
            string descricaoRetorno = "";
            bool hasOutputParameters = false;

            foreach( ParameterModel param in procedure.Parameters)
            {
                if (ignorParameters.Contains(param.Name.ToLower()) == false)
                {
                    if (param.DataType == "string")
                    {
                        procedureParameters.AppendLine("\tif( string.isNullOrEmpty(" + param.Name.Replace("@", "") + ") == false )");
                        procedureParameters.AppendLine("\t\tcmd.Parameters.Add(new SqlParameter(\"" + param.Name + "\", " + param.Name.Replace("@", "") + ") {Size = " + param.Size + ", Precision = " + param.Precision + ", Direction = " + (param.IsOutput ? "ParameterDirection.InputOutput" : "ParameterDirection.Input") + "});");
                    }
                    else if (param.IsNullable)
                    {
                        procedureParameters.AppendLine("\tif( " + param.Name.Replace("@", "") + ".HasValue )");
                        procedureParameters.AppendLine("\t\tcmd.Parameters.Add(new SqlParameter(\"" + param.Name + "\", " + param.Name.Replace("@", "") + ".Value) {Size = " + param.Size + ", Precision = " + param.Precision + ", Direction = " + (param.IsOutput ? "ParameterDirection.InputOutput" : "ParameterDirection.Input") + "});");
                    }
                    else
                    {
                        procedureParameters.AppendLine("\t\tcmd.Parameters.Add(new SqlParameter(\"" + param.Name + "\", " + param.Name.Replace("@", "") + ") {Size = " + param.Size + ", Precision = " + param.Precision + ", Direction = " + (param.IsOutput ? "ParameterDirection.InputOutput" : "ParameterDirection.Input") + "});");
                    }

                    if (param.Name.ToLower() == "@codigoretorno")
                    {
                        codigoRetorno = param.Name;
                        hasOutputParameters = true;
                    }
                    if (param.Name.ToLower() == "@descricaoretorno")
                    {
                        descricaoRetorno = param.Name;
                        hasOutputParameters = true;
                    }

                    if ((param.IsOutput == false || procedure.ReturnTable) && (param.Name.ToLower() != "@codigoretorno" && param.Name.ToLower() != "@descricaoretorno"))
                    {
                        methodParameters += methodParameters.Length == 0 ? "" : ", ";
                        if (param.IsOutput)
                            methodParameters += "out ";

                        methodParameters += param.DataType + " " + param.Name.Replace("@", "") + " " + (param.IsNullable ? " = null " : "");
                    }
                }
            }

            if( procedure.ReturnTable )
            {
                if(hasOutputParameters)
                    ret.AppendLine("public OutputTransport<DataTable> " + procedure.Name + "(" + methodParameters + ")");
                else
                    ret.AppendLine("public DataTable " + procedure.Name + "(" + methodParameters + ")");
                ret.AppendLine("{");
                ret.AppendLine("\tDataTable tmp = new DataTable();");
                ret.AppendLine("\tSqlCommand cmd = this.Connection.CreateCommand();");
                ret.AppendLine("\tcmd.CommandText = \"" + procedure.Name + "\";");
                ret.AppendLine("\tcmd.CommandType = CommandType.StoredProcedure;");
                ret.Append(procedureParameters);
                ret.AppendLine("\t");
                ret.AppendLine("\tusing (SqlDataReader dr = cmd.ExecuteReader())");
                ret.AppendLine("\t{");
                ret.AppendLine("\t\tret = new DataTable();");
                ret.AppendLine("\t\tret.Load(tmp);");
                ret.AppendLine("\t}");
                if (hasOutputParameters)
                {
                    ret.AppendLine("\tOutputTransport<DataTable> ret = new OutputTransport<DataTable>();");
                    ret.AppendLine("\tret.Data = tmp;");
                    ret.AppendLine("\tret.Code = cmd.Parameters[\"" + codigoRetorno + "\"].Value == DBNull.Value ? 0 : Convert.ToInt32(cmd.Parameters[\"" + codigoRetorno + "\"].Value);");
                    ret.AppendLine("\tret.Message = cmd.Parameters[\"" + descricaoRetorno + "\"].Value == DBNull.Value ? \"\" : cmd.Parameters[\"" + descricaoRetorno + "\"].Value.ToString();");
                    ret.AppendLine("\treturn ret;");
                }
                else
                    ret.AppendLine("\treturn tmp;");
                ret.AppendLine("}");
            }
            else 
            {
                if( hasOutputParameters)
                {
                    ret.AppendLine("public OutputTransport<string> " + procedure.Name + "(" + methodParameters + ")");
                    ret.AppendLine("{");
                    ret.AppendLine("\tOutputTransport<string> ret = new OutputTransport<string>();");
                    ret.AppendLine("\tSqlCommand cmd = this.Connection.CreateCommand();");
                    ret.AppendLine("\tcmd.CommandText = \"" + procedure.Name + "\";");
                    ret.AppendLine("\tcmd.CommandType = CommandType.StoredProcedure;");
                    ret.Append(procedureParameters);
                    ret.AppendLine("\t");
                    ret.AppendLine("\tcmd.ExecuteNonQuery();");
                    ret.AppendLine("\tret.Code = cmd.Parameters[\"" + codigoRetorno + "\"].Value == DBNull.Value ? 0 : Convert.ToInt32(cmd.Parameters[\"" + codigoRetorno+ "\"].Value);");
                    ret.AppendLine("\tret.Message = cmd.Parameters[\""+ descricaoRetorno+ "\"].Value == DBNull.Value ? \"\" : cmd.Parameters[\"" + descricaoRetorno+ "\"].Value.ToString();");
                    ret.AppendLine("\treturn ret;");
                    ret.AppendLine("}");
                }
                else
                {
                    ret.AppendLine("public void " + procedure.Name + "(" + methodParameters + ")");
                    ret.AppendLine("{");
                    ret.AppendLine("\tSqlCommand cmd = this.Connection.CreateCommand();");
                    ret.AppendLine("\tcmd.CommandText = \"" + procedure.Name + "\";");
                    ret.AppendLine("\tcmd.CommandType = CommandType.StoredProcedure;");
                    ret.Append(procedureParameters);
                    ret.AppendLine("\t");
                    ret.AppendLine("\tcmd.ExecuteNonQuery();");
                    ret.AppendLine("}");
                }
            }

            return ret.ToString();
        }
    }
}
