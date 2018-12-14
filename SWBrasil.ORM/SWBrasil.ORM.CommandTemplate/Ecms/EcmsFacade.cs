using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;

namespace SWBrasil.ORM.CommandTemplate
{
    public class EcmsFacade : CommandBase, ITableTransformation
    {
        public string CommandID
        {
            get { return "EcmsFacade"; }
        }

        public string Description
        {
            get { return "Cria Classes de Implementação"; }
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

        public override bool useGroupAsFile
        {
            get { return true; }
        }

        public string ApplyTemplate(TableModel table, List<TableModel> tables = null, string textToAppend = null)
        {
            _messages.Add(new ProjectConsoleMessages() { erro = false, data = DateTime.Now, mensagem = string.Format("{0} - Processando Tabela [{1}]", this.CommandID, table.Name) });

            string className = table.Alias.Replace("DTO", "") + "Facade";
            _fileName = className;

            StringBuilder classCode = new StringBuilder();
            if (table.IgnoreDTO)
                return "";

            string variables = "";
            string constructorParameters = "";
            string constructorVariableAssign = "";
            var dependencyTables = base.GetAllDependencies(table, tables);
            var relatedTables = base.MainTableRelations(table, tables);
            var baseConstructorParam = "";

            Dependencies(table, dependencyTables, true, out variables, out constructorParameters, out constructorVariableAssign, out baseConstructorParam);

            classCode.AppendLine("using Light;");
            classCode.AppendLine("using Light.Validation;");
            classCode.AppendLine("using SwServices;");
            classCode.AppendLine("using SwServices.Common.DTOs;");
            classCode.AppendLine("using SwServices.Common.Business;");
            classCode.AppendLine(string.Format("using {0}.Common.DTOs;", this.NameSpace));
            classCode.AppendLine(string.Format("using {0}.Data.Models;", this.NameSpace));
            classCode.AppendLine(string.Format("using {0}.Common.Business;", this.NameSpace));
            classCode.AppendLine("using System;");
            classCode.AppendLine("using System.Collections.Generic;");
            classCode.AppendLine("using System.Data;");
            classCode.AppendLine("using System.Linq;");
            classCode.AppendLine("using System.Text;");
            classCode.AppendLine("using System.Threading.Tasks;");
            classCode.AppendLine("");
            classCode.AppendLine("");
            classCode.AppendLine("namespace " + base.NameSpace + ".Core.Business");
            classCode.AppendLine("{");
            classCode.AppendLine("");

            #region BaseClass
            classCode.AppendLine("\tpublic class " + className + ": I" + table.Alias.Replace("DTO", "") + "");
            classCode.AppendLine("\t{");

            #region Variaveis Membro
            classCode.AppendLine("\t\t#region Variables");
            classCode.AppendLine(variables);
            

            classCode.AppendLine("\t\t#endregion Variables");
            classCode.AppendLine("");
            #endregion Variaveis Membro

            #region Construtor
            classCode.AppendLine("\t\t#region Construtor");

            classCode.AppendLine(string.Format("\t\tpublic {0}Base({1})", table.Alias.Replace("DTO", ""), constructorParameters));
            classCode.AppendLine("\t\t{");
            classCode.AppendLine(constructorVariableAssign);
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("\t\t#endregion Construtor");
            classCode.AppendLine("");
            #endregion Construtor

            #region Save

            classCode.AppendLine("\t\tpublic OutputTransport<" + table.Alias + "> Save(InputTransport<" + table.Alias + "> model, bool openTransaction = false)");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t\tvar ret = new OutputTransport<" + table.Alias + ">();");
            classCode.AppendLine("\t\t\t\tret.Data = model.Data;");
            classCode.AppendLine("\t\t\t\tret.success = false;");
            classCode.AppendLine("\t\t\t");
            classCode.AppendLine("\t\t\ttry");
            classCode.AppendLine("\t\t\t{");
            classCode.AppendLine("\t\t\t\t");
            classCode.AppendLine("\t\t\t\tvar data = new " + table.ModelName + "();");
            classCode.AppendLine(string.Format("\t\t\t\tSwServices.Core.Utils.Helper.CopyToBusiness<{0}, {1}>(model.Data, ref data);", table.Alias, table.ModelName));
            classCode.AppendLine("\t\t\t\t");
            var keyColumns = table.Columns.Where(c => c.IsPK).ToList();
            var strKeys = "";
            for( var i = 0; i < keyColumns.Count; i++)
            {
                strKeys += strKeys == "" ? "" : ", ";
                strKeys += "model.Data." + keyColumns[i].DTOName;
            }
            classCode.AppendLine("\t\t\t\tint affected = -1;");
            classCode.AppendLine(string.Format("\t\t\t\tvar tmp = _{0}Data.Get({1});", table.ModelName, strKeys));
            classCode.AppendLine("\t\t\t\tif( tmp == null )");
            classCode.AppendLine(string.Format("\t\t\t\t\taffected = _{0}Data.Add(data);", table.ModelName));
            classCode.AppendLine("\t\t\t\telse");
            classCode.AppendLine(string.Format("\t\t\t\t\taffected = _{0}Data.Update(data);", table.ModelName));
            classCode.AppendLine("\t\t\t\t");
            classCode.AppendLine("\t\t\t\tif(affected == 1)");
            classCode.AppendLine("\t\t\t\t{");
            if( table.Columns.Where(c=>c.IsIdentity).Count() > 0 )
            {
                classCode.AppendLine("\t\t\t\t\tOutputTransport<" + table.Alias + "> retGet = Get(data." + table.Columns.Where(c=>c.IsIdentity).First().ColumnName + ");");
                classCode.AppendLine("\t\t\t\t\tif (retGet.success)");
                classCode.AppendLine("\t\t\t\t\t\tret.Data = retGet.Data;");
                classCode.AppendLine("");
                classCode.AppendLine("");
            }
            classCode.AppendLine("\t\t\t\t\t//SAVE RELATED DATA");

            classCode.AppendLine("\t\t\t\t\tif(openTransaction)");
            classCode.AppendLine("\t\t\t\t\t\t_{0}Data.DB.Commit();".Replace("{0}", table.ModelName));
            classCode.AppendLine("\t\t\t\t\t");
            classCode.AppendLine("\t\t\t\t\tret.success = true;");
            classCode.AppendLine("\t\t\t\t\t}");
            classCode.AppendLine("\t\t\t\telse");
            classCode.AppendLine("\t\t\t\t{");
            classCode.AppendLine("\t\t\t\t\tret.code = Parameters.Default.NOTFOUND_CODE;");
            classCode.AppendLine("\t\t\t\t\tret.message = Parameters.Default.NOTFOUND_MESSAGE;");
            classCode.AppendLine("\t\t\t\t}");
            classCode.AppendLine("\t\t\t}");
            classCode.AppendLine("\t\t\tcatch (Exception err)");
            classCode.AppendLine("\t\t\t{");
            classCode.AppendLine("\t\t\t\tvar Exc = new ExceptionOutputTransport<" + table.Alias + ">();");
            classCode.AppendLine("\t\t\t\t\tExc.Exception = err;");
            classCode.AppendLine("\t\t\t\t\tExc.code = Parameters.Default.ERR_CODE;");
            classCode.AppendLine("\t\t\t\t\tExc.message = string.Format(Parameters.Default.ERR_MESSAGE, err.Message);");
            classCode.AppendLine("\t\t\t\t\tExc.success = false;");
            classCode.AppendLine("\t\t\t\t\tret = Exc;");
            classCode.AppendLine("\t\t\t\t\t");
            classCode.AppendLine("\t\t\t\t\tif(openTransaction)");
            classCode.AppendLine("\t\t\t\t\t\t_{0}Data.DB.Rollback();".Replace("{0}", table.ModelName));
            classCode.AppendLine("\t\t\t}");
            classCode.AppendLine("\t\t\treturn ret;");
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("");
            #endregion Save

            #region Search

            classCode.AppendLine("\t\tpublic TableTransport<List<" + table.Alias + ">> Search(Criteria" + table.Alias + " args)");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t\treturn Search(args, 1, 100);");
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("");

            classCode.AppendLine("\t\tpublic TableTransport<List<" + table.Alias + ">> Search(Criteria" + table.Alias + " args, int currentPage, int pageSize)");
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t\tint pageCount;");
            classCode.AppendLine("\t\t\tvar ret = new TableTransport<List<" + table.Alias + ">>();");
            classCode.AppendLine("\t\t\t    ret.Data = new List<" + table.Alias + ">();");
            classCode.AppendLine("");
            #region Criteria
            classCode.AppendLine("\t\t\t#region Criteria to Query");
            classCode.AppendLine(string.Format("\t\t\tvar strQuery = \"{0} is not null\";", table.Columns.Where(c=>c.IsPK).First().ColumnName));
            classCode.AppendLine("\t\t\tList<IParameter> parameters = new List<IParameter>();");
            for ( var i=0; i < table.Columns.Count; i++)
            {
                if( table.Columns[i].IgnoreOnDTO == false && table.Columns[i].UseAsSearchParameter)
                { 
                    switch (table.Columns[i].DataType.Trim().Replace("?", ""))
                    {
                        case "bool":
                            classCode.AppendLine(string.Format("\t\t\tif(args.{0}.HasValue)", table.Columns[i].DTOName));
                            classCode.AppendLine("\t\t\t{");
                            classCode.AppendLine(string.Format("\t\t\t\tstrQuery+= \" AND {0}=@{1}Value\";", table.Columns[i].ColumnName, table.Columns[i].DTOName));
                            classCode.AppendLine("\t\t\t\tparameters.Add(" + ParameterFromColumn(table.Columns[i], "args") + ");");
                            classCode.AppendLine("\t\t\t}");
                            classCode.AppendLine("");
                            break;

                        case "int":
                        case "long":
                        case "decimal":
                            classCode.AppendLine(string.Format("\t\t\tif(args.{0}.HasValue)", table.Columns[i].DTOName));
                            classCode.AppendLine("\t\t\t{");
                            classCode.AppendLine(string.Format("\t\t\t\tstrQuery+= \" AND {0}=@{1}Value\";", table.Columns[i].ColumnName, table.Columns[i].DTOName));
                            classCode.AppendLine("\t\t\t\tparameters.Add(" + ParameterFromColumn(table.Columns[i], "args") + ");");
                            classCode.AppendLine("\t\t\t}");
                            classCode.AppendLine("");
                            break;

                        case "Guid":
                            classCode.AppendLine(string.Format("\t\t\tif(args.{0}.HasValue)", table.Columns[i].DTOName));
                            classCode.AppendLine("\t\t\t{");
                            classCode.AppendLine(string.Format("\t\t\t\tstrQuery+= \" AND {0}=@{1}Value\";", table.Columns[i].ColumnName, table.Columns[i].DTOName));
                            classCode.AppendLine("\t\t\t\tparameters.Add(" + ParameterFromColumn(table.Columns[i], "args") + ");");
                            classCode.AppendLine("\t\t\t}");
                            classCode.AppendLine("");
                            break;

                        case "DateTime":
                            classCode.AppendLine(string.Format("\t\t\tif(args.{0}Start.HasValue)", table.Columns[i].DTOName));
                            classCode.AppendLine("\t\t\t{");
                            classCode.AppendLine(string.Format("\t\t\t\tstrQuery+= \" AND {0} >= @{1}StartValue\";", table.Columns[i].ColumnName, table.Columns[i].DTOName));
                            classCode.AppendLine("\t\t\t\tparameters.Add(" + ParameterFromColumn(table.Columns[i], "args", "Start") + ");");
                            classCode.AppendLine("\t\t\t}");
                            classCode.AppendLine("");
                            classCode.AppendLine(string.Format("\t\t\tif(args.{0}Finish.HasValue)", table.Columns[i].DTOName));
                            classCode.AppendLine("\t\t\t{");
                            classCode.AppendLine(string.Format("\t\t\t\tstrQuery+= \" AND {0} <= @{1}FinishValue\";", table.Columns[i].ColumnName, table.Columns[i].DTOName));
                            classCode.AppendLine("\t\t\t\tparameters.Add(" + ParameterFromColumn(table.Columns[i], "args", "Finish") + ");");
                            classCode.AppendLine("\t\t\t}");
                            classCode.AppendLine("");
                            break;

                        default:
                            classCode.AppendLine(string.Format("\t\t\tif(string.IsNullOrEmpty(args.{0}) == false)", table.Columns[i].DTOName));
                            classCode.AppendLine("\t\t\t{");
                            classCode.AppendLine(string.Format("\t\t\t\tstrQuery+= \" AND {0} LIKE '%' + @{1}Value + '%'\";", table.Columns[i].ColumnName, table.Columns[i].DTOName));
                            classCode.AppendLine("\t\t\t\tparameters.Add(" + ParameterFromColumn(table.Columns[i], "args") + ");");
                            classCode.AppendLine("\t\t\t}");
                            classCode.AppendLine("");
                            break;
                    }
                }
            }
            classCode.AppendLine("\t\t\t#endregion Criteria to Query");
            #endregion Criteria
            classCode.AppendLine("");
            classCode.AppendLine("\t\t\ttry");
            classCode.AppendLine("\t\t\t{");
            classCode.AppendLine(string.Format("\t\t\t\tvar lstTmp = _{0}Data.List(new Query(strQuery, parameters.ToArray()), pageSize, currentPage, out pageCount);", table.ModelName));
            classCode.AppendLine("\t\t\t\tif (lstTmp != null)");
            classCode.AppendLine("\t\t\t\t{");
            classCode.AppendLine("\t\t\t\t\tforeach (var tmp in lstTmp)");
            classCode.AppendLine("\t\t\t\t\t{");
            classCode.AppendLine("\t\t\t\t\t\tvar dto = new " + table.Alias + "();");
            classCode.AppendLine(string.Format("\t\t\t\t\t\tSwServices.Core.Utils.Helper.CopyFromBusiness<{0}, {1}>(tmp, ref dto);", table.Alias, table.ModelName));
            classCode.AppendLine("\t\t\t\t\t\tLoadRelatedData(ref dto);");
            classCode.AppendLine("\t\t\t\t\t\tret.Data.Add(dto);");
            classCode.AppendLine("\t\t\t\t\t}");
            classCode.AppendLine("\t\t\t\t}");
            classCode.AppendLine("");
            classCode.AppendLine("\t\t\t\tret.pages = pageCount;");
            classCode.AppendLine("\t\t\t\tret.success = true;");
            classCode.AppendLine("\t\t\t}");
            classCode.AppendLine("\t\t\tcatch(Exception err)");
            classCode.AppendLine("\t\t\t{");
            classCode.AppendLine("\t\t\t\tret.Exception = err;");
            classCode.AppendLine("\t\t\t\tret.success = false;");
            classCode.AppendLine("\t\t\t\tret.code = Parameters.Default.ERR_CODE;");
            classCode.AppendLine("\t\t\t\tret.message = string.Format(Parameters.Default.ERR_MESSAGE, err.Message);");
            classCode.AppendLine("\t\t\t}");
            classCode.AppendLine("\t\t\treturn ret;");
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("");
            #endregion Save

            #region Protected Methods

            classCode.AppendLine("\t\tprotected void LoadRelatedData(ref " + table.Alias + " dto)");
            classCode.AppendLine("\t\t{");
            var relatedColumns = table.Columns.Where(c => string.IsNullOrEmpty(c.RelatedTable) == false && c.IgnoreOnDTO == false).ToList();
            for (var i = 0; i < relatedColumns.Count; i++)
            {
                var relatedTable = tables.Where(t => t.Name == relatedColumns[i].RelatedTable).Single();
                var dtoName = "dto" + relatedTable.Alias.Replace("DTO", "");
                var modelName = "result" + relatedTable.ModelName;

                if( relatedTable.MainDTO )
                {
                    dtoName = relatedTable.Alias.Replace("DTO", "");
                    classCode.AppendLine(string.Format("\t\t\tvar result{0} = _{1}BS.Get( dto.{2}.{3} );", dtoName, dtoName, dtoName, relatedTable.Columns.Where(c => c.IsPK).First().DTOName));
                    classCode.AppendLine("\t\t\tif( result{0}.success ) ".Replace("{0}", dtoName));
                    classCode.AppendLine(string.Format("\t\t\t\tdto.{0} = result{1}.Data;", dtoName, dtoName));
                    classCode.AppendLine("");
                }
                else
                {
                    classCode.AppendLine(string.Format("\t\t\tvar {0} = new {1}();", dtoName, relatedTable.Alias));
                    classCode.AppendLine(string.Format("\t\t\tvar {0} = _{1}Data.Get( dto.{2}.{3} );", modelName, relatedTable.ModelName, relatedTable.Alias.Replace("DTO", ""), relatedTable.Columns.Where(c => c.IsPK).First().DTOName));
                    classCode.AppendLine("\t\t\tif( {0}.success ) {".Replace("{0}", modelName));
                    classCode.AppendLine(string.Format("\t\t\t\tUtils.Helper.CopyFromBusiness<{0}, {1}>({2}.Data, ref {3});", relatedTable.Alias, (relatedTable.ModelName + "Model"), modelName, dtoName));
                    classCode.AppendLine(string.Format("\t\t\t\tdto.{0} = {1};", relatedTable.Alias.Replace("DTO", ""), dtoName));
                    classCode.AppendLine("\t\t\t}");
                    classCode.AppendLine("");
                }
                
            }

            for (var i = 0; i < relatedTables.Count; i++)
            {
                var columnPK = table.Columns.Where(c => c.IsPK).First().ColumnName;
                var queryStr = "\"" + columnPK + "='\" + model." + columnPK + " + \"'\"";

                if ( relatedTables[i].Type == enumTableType.Dictionary )
                {
                    var keyColumn = relatedTables[i].Columns.Where(c => c.IgnoreOnDTO == false && c.IsUniqueKey).FirstOrDefault();
                    if (keyColumn == null)
                    {
                        _messages.Add(new ProjectConsoleMessages() { erro = true, data = DateTime.Now, mensagem = string.Format("Key Column not found on Dicionary Table [{0}]", relatedTables[i].Name) });
                        continue;
                    }

                    var valColumn = relatedTables[i].Columns.Where(c => c.IgnoreOnDTO == false && c.IsUniqueKey == false && c.ColumnName != keyColumn.ColumnName).FirstOrDefault();
                    if (valColumn == null)
                    {
                        _messages.Add(new ProjectConsoleMessages() { erro = true, data = DateTime.Now, mensagem = string.Format("Value Column not found on Dicionary Table [{0}]", relatedTables[i].Name) });
                        continue;
                    }

                    var varName = "lstDic" + i.ToString("00");

                    classCode.AppendLine(string.Format("\t\t\tdto.{0}s = new Dictionary<{1}, {2}>();", relatedTables[i].Alias.Replace("DTO", ""), keyColumn.DataType, valColumn.DataType));
                    classCode.AppendLine(string.Format("\t\t\tvar {0} = _{1}Data.List( new Query({2}) );", varName, relatedTables[i].ModelName, queryStr));
                    classCode.AppendLine(string.Format("\t\t\tfor( var i=0; i < {0}.Count; i++)", varName));
                    classCode.AppendLine(string.Format("\t\t\t\tdto.{0}s.Add({1}[i].{2}, {3}[i].{4});", relatedTables[i].Alias.Replace("DTO", ""), varName, keyColumn.ColumnName, varName, valColumn.ColumnName));
                    classCode.AppendLine("");
                }
                else
                {
                    var dtoName = "dto" + relatedTables[i].Alias.Replace("DTO", "");
                    var varName = "lstTemp" + i.ToString("00");
                    var dtoType = relatedTables[i].Alias;
                    var singleColumnDTO = relatedTables[i].Columns.Where(c => c.IgnoreOnDTO == false).FirstOrDefault();
                    if (relatedTables[i].Columns.Where(c => c.IgnoreOnDTO == false).Count() == 1)
                        dtoType = singleColumnDTO.DataType;

                    classCode.AppendLine(string.Format("\t\t\tdto.{0}s = new List<{1}>();", relatedTables[i].Alias.Replace("DTO", ""), dtoType));
                    classCode.AppendLine(string.Format("\t\t\tvar {0} = _{1}Data.List( new Query({2}) );", varName, relatedTables[i].ModelName, queryStr));
                    classCode.AppendLine(string.Format("\t\t\tfor( var i=0; i < {0}.Count; i++)", varName));
                    classCode.AppendLine("\t\t\t{");
                    if (relatedTables[i].Columns.Where(c => c.IgnoreOnDTO == false).Count() == 1)
                    {
                        classCode.AppendLine(string.Format("\t\t\t\tdto.{0}s.Add({1}[i].{2});", relatedTables[i].Alias.Replace("DTO", ""), varName, singleColumnDTO.ColumnName));
                    }
                    else
                    {
                        classCode.AppendLine(string.Format("\t\t\t\tvar {0} = new {1}();", dtoName, relatedTables[i].Alias));
                        classCode.AppendLine(string.Format("\t\t\t\tUtils.Helper.CopyFromBusiness<{0}, {1}>({2}[i], ref {3});", relatedTables[i].Alias, (relatedTables[i].ModelName + "Model"), varName, dtoName));
                        classCode.AppendLine("");

                        LoadRelatedData(dtoName, varName + "[i]", relatedTables[i], tables, ref classCode);

                        classCode.AppendLine(string.Format("\t\t\t\tdto.{0}s.Add({1});", relatedTables[i].Alias.Replace("DTO", ""), dtoName));
                    }
                    classCode.AppendLine("\t\t\t}");
                    classCode.AppendLine("");
                }

            }

            classCode.AppendLine("\t\t}");
            classCode.AppendLine("");

            #endregion

            #region By PK

            var pkColumn = table.Columns.Where(c => c.IsPK).FirstOrDefault();
            if (pkColumn != null)
            {
                string methodGet = "";
                string methodDel = "";
                string parameterNames = "";

                var pks = table.Columns.Where(c => c.IsPK).ToList();
                if (pks != null)
                {
                    
                    for (var i = 0; i < pks.Count; i++)
                    {
                        methodGet += methodGet != "" ? ", " : "";
                        methodGet += pks[i].DataType.Replace("?", "") + " " + pks[i].DTOName.ToLowerInvariant();
                        methodDel += methodDel != "" ? ", " : "";
                        methodDel += pks[i].DataType.Replace("?", "") + " " + pks[i].DTOName.ToLowerInvariant();

                        parameterNames += parameterNames != "" ? ", " : "";
                        parameterNames += pks[i].DTOName.ToLowerInvariant();
                    }
                }

                classCode.AppendLine("\t\tpublic OutputTransport<" + table.Alias + "> Get(" + methodGet + ")");
                classCode.AppendLine("\t\t{");
                classCode.AppendLine("\t\t\tvar ret = new OutputTransport<" + table.Alias + ">();");
                classCode.AppendLine("\t\t\tvar model = _" + table.ModelName + "Data.Get(" + parameterNames + ");");
                classCode.AppendLine("\t\t\tif( model == null )");
                classCode.AppendLine("\t\t\t{");
                classCode.AppendLine("\t\t\t\tret.code = Parameters.Default.NOTFOUND_CODE;");
                classCode.AppendLine("\t\t\t\tret.message = Parameters.Default.NOTFOUND_MESSAGE;");
                classCode.AppendLine("\t\t\t\tret.success = false;");
                classCode.AppendLine("\t\t\t}");
                classCode.AppendLine("\t\t\telse");
                classCode.AppendLine("\t\t\t{");
                classCode.AppendLine("\t\t\t\tvar dto = new " + table.Alias + "();");
                classCode.AppendLine("\t\t\t\tSwServices.Core.Utils.Helper.CopyFromBusiness<" + table.Alias + ", " + table.ModelName + ">(model, ref dto);");
                classCode.AppendLine("\t\t\t\tLoadRelatedData(ref dto);");
                classCode.AppendLine("\t\t\t\tret.Data = dto;");
                classCode.AppendLine("\t\t\t\tret.success = true;");
                classCode.AppendLine("\t\t\t}");
                classCode.AppendLine("\t\t\treturn ret;");
                classCode.AppendLine("\t\t}");
                classCode.AppendLine("");

                classCode.AppendLine("\t\tpublic OutputTransport<string> Delete(" + methodDel + ")");
                classCode.AppendLine("\t\t{");
                classCode.AppendLine("\t\t\tvar ret = new OutputTransport<string>();");
                classCode.AppendLine("\t\t\ttry");
                classCode.AppendLine("\t\t\t{");
                classCode.AppendLine("\t\t\t\tvar model = _" + table.ModelName + "Data.Get(" + parameterNames + ");");
                classCode.AppendLine("\t\t\t\t_" + table.ModelName + "Data.Delete(model);");
                classCode.AppendLine("\t\t\t\tret.success = true;");
                classCode.AppendLine("\t\t\t}");
                classCode.AppendLine("\t\t\tcatch(Exception err)");
                classCode.AppendLine("\t\t\t{");
                classCode.AppendLine("\t\t\t\tvar exc = new ExceptionOutputTransport<string>();");
                classCode.AppendLine("\t\t\t\texc.Exception = err;");
                classCode.AppendLine("\t\t\t\texc.success = false;");
                classCode.AppendLine("\t\t\t\texc.code = Parameters.Default.ERR_CODE;");
                classCode.AppendLine("\t\t\t\texc.message = string.Format(Parameters.Default.ERR_MESSAGE, err.Message);");
                classCode.AppendLine("\t\t\t\tret = exc;");
                classCode.AppendLine("\t\t\t}");
                classCode.AppendLine("\t\t\treturn ret;");
                classCode.AppendLine("\t\t}");
                classCode.AppendLine("");
            }

            #endregion By PK

            #region By Unique Key
            var uniqueKey = "";
            var uniqueQuery = "";
            var uniqueKeyColumns = table.Columns.Where(c => c.IsUniqueKey).ToList();
            if (uniqueKeyColumns != null)
            {
                for (var i = 0; i < uniqueKeyColumns.Count; i++)
                {
                    uniqueKey += (uniqueKey != "" ? ", " : "");
                    uniqueKey += uniqueKeyColumns[i].DataType;
                    uniqueKey += " ";
                    uniqueKey += uniqueKeyColumns[i].DTOName;

                    uniqueQuery += uniqueQuery == "" ? "" : " AND ";
                    uniqueQuery += uniqueKeyColumns[i].ColumnName + "='\" + " + uniqueKeyColumns[i].DTOName + " + \"'";
                }

                if (string.IsNullOrEmpty(uniqueKey) == false)
                {
                    classCode.AppendLine("\t\tpublic OutputTransport<" + table.Alias + "> Get(" + uniqueKey + ")");
                    classCode.AppendLine("\t\t{");
                    classCode.AppendLine("\t\t\tvar ret = new OutputTransport<" + table.Alias + ">();");
                    classCode.AppendLine("\t\t\tvar lst = _" + table.ModelName + "Data.List(new Query(\"" + uniqueQuery + "\"));");
                    classCode.AppendLine("\t\t\tif( lst != null && lst.Count > 0 )");
                    classCode.AppendLine("\t\t\t{");
                    classCode.AppendLine("\t\t\t\tvar dto = new " + table.Alias + "();");
                    classCode.AppendLine("\t\t\t\tvar model = lst.First();");
                    classCode.AppendLine("\t\t\t\tUtils.Helper.CopyFromBusiness<" + table.Alias + ", " + table.ModelName + "Model>(model, ref dto);");
                    classCode.AppendLine("\t\t\t\tLoadRelatedData(ref dto);");
                    classCode.AppendLine("\t\t\t\tret.Data = dto;");
                    classCode.AppendLine("\t\t\t\tret.success = true;");
                    classCode.AppendLine("\t\t\t}");
                    classCode.AppendLine("\t\t\telse");
                    classCode.AppendLine("\t\t\t{");
                    classCode.AppendLine("\t\t\t\tret.code = Parameters.Default.NOTFOUND_CODE;");
                    classCode.AppendLine("\t\t\t\tret.message = Parameters.Default.NOTFOUND_MESSAGE;");
                    classCode.AppendLine("\t\t\t\tret.success = false;");
                    classCode.AppendLine("\t\t\t}");
                    classCode.AppendLine("\t\t\treturn ret;");
                    classCode.AppendLine("\t\t}");
                    classCode.AppendLine("");
                }
            }
            #endregion By Unique Key

            classCode.AppendLine("\t}");
            classCode.AppendLine("");
            classCode.AppendLine("");
            #endregion BaseClass

            #region Business Class

            classCode.AppendLine("\tpublic class " + table.Alias.Replace("DTO", "") + ": " + className + "");
            classCode.AppendLine("\t{");
            #region Construtor
            classCode.AppendLine("\t\t#region Construtor");
            classCode.AppendLine(string.Format("\t\tpublic {0}({1}) : base({2})", table.Alias.Replace("DTO", ""), ("" + constructorParameters), baseConstructorParam));
            classCode.AppendLine("\t\t{");
            classCode.AppendLine("\t\t}");
            classCode.AppendLine("\t\t#endregion Construtor");
            classCode.AppendLine("");
            #endregion Construtor
            classCode.AppendLine("\t}");
            classCode.AppendLine("");
            classCode.AppendLine("");

            #endregion Business Class

            classCode.AppendLine("}");
            
            return classCode.ToString();
        }


        protected void LoadRelatedData(string dtoObject, string modelObject, TableModel table, List<TableModel> tables, ref StringBuilder classCode)
        {
            var columns = table.Columns.Where(c => string.IsNullOrEmpty(c.RelatedTable) == false && c.IgnoreOnDTO == false).ToList();
            for( var i=0; i < columns.Count; i++)
            {
                var relatedTable = tables.Where(t => t.Name == columns[i].RelatedTable).Single();
                var keys = "";
                var pkColumns = relatedTable.Columns.Where(c => c.IsPK).ToList();
                for(var c=0; c < pkColumns.Count; c++)
                {
                    keys += keys == "" ? "" : ", ";
                    keys += modelObject + "." + pkColumns[c].ColumnName;
                }

                if(relatedTable.MainDTO)
                {
                    var dto = relatedTable.Alias.Replace("DTO", "");
                    classCode.AppendLine(string.Format("\t\t\t\t\tvar result{0} = _{1}BS.Get({2});", dto, dto, keys));
                    classCode.AppendLine(string.Format("\t\t\t\t\tif( result{0}.success )", dto));
                    classCode.AppendLine(string.Format("\t\t\t\t\t\t{0}.{1} = result{2}.Data;", dtoObject, columns[i].DTOName, dto));
                    classCode.AppendLine("");
                }
                else
                {
                    classCode.AppendLine(string.Format("\t\t\t\t\tvar dto{0} = new {1}();", relatedTable.Alias.Replace("DTO", ""), relatedTable.Alias));
                    classCode.AppendLine(string.Format("\t\t\t\t\tvar model{0} = _{1}Data.Get({2});", relatedTable.Alias.Replace("DTO", ""), relatedTable.ModelName, keys));
                    classCode.AppendLine(string.Format("\t\t\t\t\tUtils.Helper.CopyFromBusiness<{0}, {1}Model>({2}, ref {3});", relatedTable.Alias, relatedTable.ModelName, ("model" + relatedTable.Alias.Replace("DTO", "")), ("dto" + relatedTable.Alias.Replace("DTO", ""))));
                    classCode.AppendLine(string.Format("\t\t\t\t\t{0}.{1} = {2};", dtoObject, relatedTable.Alias.Replace("DTO", "") + "", "dto" + relatedTable.Alias.Replace("DTO", "")));
                    classCode.AppendLine("");
                }
            }
        }
    }
}

