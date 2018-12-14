using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWBrasil.ORM.Common;
using System.Globalization;

namespace SWBrasil.ORM.CommandTemplate
{
    public abstract class CommandBase
    {
        public string ProjectName { get; set; }
        public string NameSpace { get; set; }
        public string ConnectionStringID { get; set; }

        protected string _fileName = "";
        protected string _directoryName = "";
        protected ColumnModel RelatedColumn(ColumnModel column, List<TableModel> tables)
        {
            ColumnModel ret= null;
            if (tables != null)
            {
                var rTable = tables.Where(t => t.Name == column.RelatedTable).FirstOrDefault();
                if (rTable != null)
                {
                    var key = rTable.Columns.Where(c => c.IsPK == true).FirstOrDefault();
                    if (key != null)
                    {
                        if (column.DataType.Replace("?", "") == key.DataType.Replace("?", ""))
                            ret = key;
                        else
                        {
                            key = rTable.Columns.Where(c => c.IsUniqueKey == true).FirstOrDefault();
                            if (key != null && column.DataType == key.DataType)
                                ret = key;
                        }
                    }
                }
            }

            return ret;
        }

        protected string ParameterFromColumn(ColumnModel column, string propertyPrefix, string propertySufix = "")
        {
            string ret = "";
            string template = " new Light.Parameter() { ParameterName = \"@{0}{3}Value\", DbType = System.Data.DbType.{1}, Value = {2}.{0}{5} {4} }".Replace("{2}", propertyPrefix).Replace("{0}", column.DTOName);

            switch (column.DataType.Trim().ToUpper().Replace("?", ""))
            {
                case "INT":
                    ret = template.Replace("{1}", "Int32");
                    ret = ret.Replace("{5}", ".Value");
                    break;

                case "LONG":
                    ret = template.Replace("{1}", "Int64");
                    ret = ret.Replace("{5}", ".Value");
                    break;

                case "GUID":
                    ret = template.Replace("{1}", "Guid");
                    ret = ret.Replace("{5}", ".Value");
                    break;

                case "DATETIME":
                    ret = template.Replace("{1}", "DateTime");
                    ret = ret.Replace("{3}", propertySufix);
                    ret = ret.Replace("{5}", propertySufix + ".Value");
                    break;

                case "BOOL":
                    ret = template.Replace("{1}", "Boolean");
                    ret = ret.Replace("{5}", ".Value");
                    break;

                default:
                    ret = template.Replace("{1}", "String");
                    ret = ret.Replace("{4}", (", Size = " + column.Size));
                    break;
            }

            return ret.Replace("{4}", "").Replace("{3}", "").Replace("{5}", "");
        }

        protected TableModel RelatedTable(ColumnModel column, List<TableModel> tables)
        {
            TableModel ret = null;
            if (tables != null)
            {
                var rTable = tables.Where(t => t.Name == column.RelatedTable).FirstOrDefault();
                if (rTable != null)
                    ret = rTable;
            }

            return ret;
        }

        protected List<TableModel> GetAllDependencies(TableModel currentTable, List<TableModel> tables)
        {
            List<TableModel> ret = MainTableRelations(currentTable, tables);
            ret.Add(currentTable);
            for( var i=0; i < ret.Count; i++ )
            {
                var columns = ret[i].Columns.Where(c => string.IsNullOrEmpty(c.RelatedTable) == false && c.IgnoreOnDTO == false).ToList();
                for( var c=0; c < columns.Count; c++ )
                {
                    if( ret.Where(t=>t.Name == columns[c].RelatedTable).Count() == 0 )
                        ret.Add(tables.Where(t => t.Name == columns[c].RelatedTable).Single());
                }
            }
            

            return ret;
        }

        protected List<TableModel> MainTableRelations(TableModel mainTable, List<TableModel> tables)
        {
            var tbls = tables.Select(t => new TableModel
            {
                Alias = t.Alias,
                Name = t.Name,
                Group = t.Group,
                IgnoreDTO = t.IgnoreDTO,
                MainDTO = t.MainDTO,
                Type = t.Type,
                Columns = t.Columns.Select(column => new ColumnModel
                {
                    ColumnName = column.ColumnName,
                    DataType = column.DataType,
                    DbType = column.DbType,
                    DefaultValue = column.DefaultValue,
                    DTOName = column.DTOName,
                    IgnoreOnDTO = column.IgnoreOnDTO,
                    ExtendedProperty = column.ExtendedProperty,
                    IsIdentity = column.IsIdentity,
                    IsPK = column.IsPK,
                    IsUniqueKey = column.IsUniqueKey,
                    Precision = column.Precision,
                    RelatedTable = column.RelatedTable,
                    Required = column.Required,
                    SelectionType = column.SelectionType,
                    ShowOnResultGrid = column.ShowOnResultGrid,
                    Size = column.Size,
                    UseAsLabelOnComboBox = column.UseAsLabelOnComboBox,
                    UseAsSearchParameter = column.UseAsSearchParameter,
                    UseAsRelatedObject = column.UseAsRelatedObject,
                }).Where(c => c.RelatedTable == mainTable.Name).ToList()
            }).Where(t => t.MainDTO == false && t.Columns.Count > 0 && t.Group == mainTable.Group).ToList();

            List<TableModel> ret = new List<TableModel>();
            foreach(var tbl in tbls)
                ret.Add(tables.Where(t => t.Name == tbl.Name).SingleOrDefault());
            
            return ret;
        }
        public virtual bool useGroupAsFile { get { return false; } }

        protected virtual string DataTypeToDbType(string dataType)
        {
            string ret = dataType;

            switch (ret.Trim().ToUpper().Replace("?", ""))
            {
                case "STRING":
                    ret = "DbType.String";
                    break;

                case "DATETIME":
                    ret = "DbType.DateTime";
                    break;

                case "INT":
                    ret = "DbType.Int32";
                    break;

                case "SHORT":
                    ret = "DbType.Int16";
                    break;

                case "LONG":
                    ret = "DbType.Int64";
                    break;

                case "DECIMAL":
                    ret = "DbType.Decimal";
                    break;

                case "GUID":
                    ret = "DbType.Guid";
                    break;

                case "BOOL":
                case "BIT":
                    ret = "DbType.Boolean";
                    break;
            }
            return ret;
        }

        protected List<ProjectConsoleMessages> _messages = new List<ProjectConsoleMessages>();
        public List<ProjectConsoleMessages> Mensagens
        {
            get { return _messages; }
        }

        protected void Dependencies(TableModel currentTable, List<TableModel> tables, bool isImplementation, out string variables, out string constructorParameters, out string constructorVariableAssign, out string baseConstructorParameter)
        {
            variables = "";
            baseConstructorParameter = "";
            constructorParameters = "";
            constructorVariableAssign = "";
            for (var i = 0; i < tables.Count; i++)
            {
                if (tables[i].MainDTO && (tables[i].Name != currentTable.Name || isImplementation == false))
                {
                    string dto = tables[i].Alias.Replace("DTO", "");
                    variables += "\t\tprotected I{0} _{0}BS = null;".Replace("{0}", dto) + Environment.NewLine;
                    baseConstructorParameter += baseConstructorParameter == "" ? "" : ", ";
                    baseConstructorParameter += "i" + dto;

                    constructorParameters += constructorParameters == "" ? "" : ", ";
                    constructorParameters += "I" + dto + " i" + dto;

                    constructorVariableAssign += "\t\t\tif(i{0} == null)".Replace("{0}", dto) + Environment.NewLine;
                    constructorVariableAssign += "\t\t\t\tthrow new ArgumentNullException(\"i{0}\");".Replace("{0}", dto) + Environment.NewLine;
                    constructorVariableAssign += "\t\t\t_{0}BS = i{0};".Replace("{0}", dto) + Environment.NewLine;
                }
                else
                {
                    variables += "\t\tprotected IDAL<{0}> _{0}Data = null;".Replace("{0}", tables[i].ModelName) + Environment.NewLine;

                    baseConstructorParameter += baseConstructorParameter == "" ? "" : ", ";
                    baseConstructorParameter += "dal" + tables[i].ModelName;

                    constructorParameters += constructorParameters == "" ? "" : ", ";
                    constructorParameters += "IDAL<" + tables[i].ModelName + "> dal" + tables[i].ModelName;

                    constructorVariableAssign += "\t\t\tif(dal{0} == null)".Replace("{0}", tables[i].ModelName) + Environment.NewLine;
                    constructorVariableAssign += "\t\t\t\tthrow new ArgumentNullException(\"dal{0}\");".Replace("{0}", tables[i].ModelName) + Environment.NewLine;
                    constructorVariableAssign += "\t\t\t_{0}Data = dal{0};".Replace("{0}", tables[i].ModelName) + Environment.NewLine;
                }
            }
        }

        protected string DeCapitalize(string capitalizedText)
        {
            string deCapitalizedText = capitalizedText;
            if (string.IsNullOrEmpty(capitalizedText) == false)
            {
                string first = capitalizedText.Substring(0, 1).ToLower();
                deCapitalizedText = first + capitalizedText.Substring(1);
            }

            return deCapitalizedText;
        }

        protected string Capitalize(string decapitalizedText)
        {
            string capitalizedText = decapitalizedText;
            if (string.IsNullOrEmpty(decapitalizedText) == false)
            {
                string first = decapitalizedText.Substring(0, 1).ToUpper();
                capitalizedText = first + decapitalizedText.Substring(1);
            }

            return capitalizedText;
        }
    }
}
