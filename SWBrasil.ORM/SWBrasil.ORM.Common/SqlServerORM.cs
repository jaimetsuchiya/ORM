using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWBrasil.ORM.Common
{
    public class SqlServerORM: BaseORM
    {
        protected string QUERY_PROCEDURES = "SELECT NAME FROM SYS.PROCEDURES ORDER BY NAME";
        protected string PROC_PARAMETERS = "SELECT p.name AS Parameter,t.name AS [Type],p.default_value,p.max_length,p.precision,p.is_nullable,p.is_output,case when exists(SELECT 1 FROM sys.dm_exec_describe_first_result_set_for_object(OBJECT_ID('{0}'), 0)) then 1 else 0 end as ReturnTable FROM sys.procedures sp JOIN sys.parameters p ON sp.object_id = p.object_id JOIN sys.types t ON p.system_type_id = t.system_type_id WHERE sp.name = '{0}' ORDER BY is_output, Parameter";
        protected string QUERY_COLUMNS = @"SELECT DISTINCT C.COLUMN_NAME, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE, case when tblPK.IndexName is null then 0 else 1 end as PK, case when tblUK.ColName is null then 0 else 1 end as UniqueKey, tblFK.ReferenceTableName,COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') as IsIdentity, C.COLUMN_DEFAULT, EP.[Extended Property] 
                                            FROM  INFORMATION_SCHEMA.COLUMNS C 
                                            LEFT JOIN (SELECT  i.name AS IndexName, OBJECT_NAME(ic.OBJECT_ID) AS TableName, COL_NAME(ic.OBJECT_ID,ic.column_id) AS ColumnName FROM    sys.indexes AS i INNER JOIN sys.index_columns AS ic ON  i.OBJECT_ID = ic.OBJECT_ID AND i.index_id = ic.index_id WHERE   i.is_primary_key = 1 ) as tblPK on tblPK.ColumnName = C.COLUMN_NAME and tblPK.TableName = C.TABLE_NAME 
                                            LEFT JOIN (SELECT f.name AS ForeignKey, OBJECT_NAME(f.parent_object_id) AS TableName, COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName, OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName, COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferenceColumnName FROM sys.foreign_keys AS f INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id) as tblFK ON tblFK.TableName = C.TABLE_NAME AND tblFK.ColumnName = C.COLUMN_NAME 
                                            LEFT JOIN (SELECT col.name as ColName, obj.name as TableName FROM sys.objects AS obj INNER JOIN sys.columns AS col ON col.object_id = obj.object_id INNER JOIN sys.index_columns AS idx_cols ON idx_cols.column_id = col.column_id AND idx_cols.object_id = col.object_id INNER JOIN sys.indexes AS idx ON idx_cols.index_id = idx.index_id AND idx.object_id = col.object_id WHERE idx.is_unique = 1 AND idx.is_primary_key = 0) as tblUK ON tblUK.Colname = C.COLUMN_NAME AND tblUK.TableName = C.TABLE_NAME
                                            LEFT JOIN (SELECT major_id, minor_id, ep.name, t.name AS [Table], c.name AS [Column], value AS [Extended Property]  FROM sys.extended_properties AS ep  INNER JOIN sys.tables AS t ON ep.major_id = t.object_id    INNER JOIN sys.columns AS c ON ep.major_id = c.object_id AND ep.minor_id = c.column_id   WHERE class = 1 ) as ep ON ep.[Table] = C.TABLE_NAME AND ep.[Column] = C.COLUMN_NAME
                                            WHERE C.TABLE_NAME = N'{0}'";
        protected string QUERY_TABLES = "SELECT t.name AS [Table], value AS [Extended Property], ep.name as [Extended Property Type]  FROM sys.tables AS t LEFT JOIN sys.extended_properties AS ep  ON ep.major_id = t.object_id and class = 1 and ep.minor_id = 0 ORDER BY t.name";

        public override bool Connect(string connectionString)
        {
            bool ret = false;
            this.Tables = new List<TableModel>();
            this.Procedures = new List<ProcModel>();

            try
            {
                using (SqlConnection cnx = new SqlConnection(connectionString))
                {
                    cnx.Open();

                    SqlCommand cmd = new SqlCommand(QUERY_TABLES, cnx);
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader rdr = cmd.ExecuteReader();
                    List<TableModel> _tables = new List<TableModel>();

                    while (rdr.Read())
                        _tables.Add(new TableModel() { Name = rdr.GetString(0), Group = (rdr.IsDBNull(1) == false && rdr.GetString(2) == "MS_Group" ? rdr.GetString(1) : null) });
                    rdr.Close();

                    foreach (TableModel table in _tables)
                        this.Tables.Add(createTableModel(table, cnx));


                    cmd = new SqlCommand(QUERY_PROCEDURES, cnx);
                    cmd.CommandType = CommandType.Text;
                    rdr = cmd.ExecuteReader();
                    List<string> _procedures = new List<string>();
                    while (rdr.Read())
                        _procedures.Add(rdr.GetString(0));
                    rdr.Close();

                    foreach (string proc in _procedures)
                        this.Procedures.Add(createProcModel(proc, cnx));


                    cnx.Close();
                }
            }
            catch(Exception err)
            {
                Console.Write(err.Message);
            }

            ret = this.Tables.Count > 0;
            return ret;

        }

        protected virtual ProcModel createProcModel(string procModel, SqlConnection connection)
        {
            ProcModel ret = new ProcModel();
            ret.Parameters = new List<ParameterModel>();
            ret.Name = procModel;

            SqlCommand cmd = new SqlCommand(string.Format(PROC_PARAMETERS, procModel), connection);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                ret.ReturnTable = rdr.GetInt32(7) == 1;
                ParameterModel param = new ParameterModel();
                param.Name = rdr.GetString(0);
                param.DbType = rdr.GetString(1);
                param.IsNullable = rdr.GetBoolean(5);
                param.IsOutput = rdr.GetBoolean(6);
                
                if (param.DbType.Contains("int") == false && (rdr.IsDBNull(3) == false || rdr.IsDBNull(4) == false))
                    param.Size = rdr.IsDBNull(3) == false ? Convert.ToInt32(rdr.GetSqlValue(3).ToString()) : Convert.ToInt32(rdr.GetSqlValue(4).ToString());
                else
                    param.Size = null;

                if (rdr.IsDBNull(4) == false)
                    param.Precision = rdr.GetByte(4);
                else
                    param.Precision = null;

                param.DataType = ConvertDataType(param.DbType, param.IsNullable);
                
                ret.Parameters.Add(param);
            }
            rdr.Close();

            return ret;
        }

        protected virtual TableModel createTableModel(TableModel table, SqlConnection connection)
        {
            TableModel ret = new TableModel();
            ret.Columns = new List<ColumnModel>();
            ret.Name = table.Name;
            ret.Group = table.Group;

            SqlCommand cmd = new SqlCommand(string.Format(QUERY_COLUMNS, table.Name), connection);
            cmd.CommandType = CommandType.Text;
            SqlDataReader rdr = cmd.ExecuteReader();
            int pos = 1;
            while (rdr.Read())
            {
                ColumnModel column = new ColumnModel();
                column.ColumnName = rdr.GetString(0);
                column.DbType = rdr.GetString(2);
                column.Position = pos;
                pos++;
                if( column.DbType.Contains("int") == false && ( rdr.IsDBNull(3) == false  || rdr.IsDBNull(4) == false ))
                    column.Size = rdr.IsDBNull(3) == false ? Convert.ToInt32( rdr.GetSqlValue(3).ToString() ): Convert.ToInt32( rdr.GetSqlValue(4).ToString() );
                else
                    column.Size = null;


                if( rdr.IsDBNull(5) == false )
                    column.Precision = rdr.GetInt32(5);
                else
                    column.Precision = null;

                column.DataType = ConvertDataType(column.DbType, !column.Required);
                column.IsPK = (rdr.GetInt32(6) == 1);
                column.IsUniqueKey = (rdr.GetInt32(7) == 1);

                column.Required = rdr.GetString(1) == "NO";
                if (rdr.IsDBNull(8) == false)
                    column.RelatedTable = rdr.GetString(8);

                if (rdr.IsDBNull(9) == false)
                    column.IsIdentity = (rdr.GetInt32(9) == 1);

                if (rdr.IsDBNull(10) == false)
                    column.DefaultValue = rdr.GetString(10);

                if (rdr.IsDBNull(11) == false)
                    column.ExtendedProperty = rdr.GetString(11);

                ret.Columns.Add(column);
            }
            rdr.Close();

            return ret;
        }

        public override string ConvertDataType(string dataBaseType, bool nullable)
        {
            string ret = "string";

            switch (dataBaseType.Trim().ToUpper())
            {
                case "BIGINT":
                    ret = ("long");
                    break;

                case "INT":
                    ret = ("int");
                    break;

                case "BIT":
                    ret = ("bool");
                    break;

                case "SMALLINT":
                    ret = ("short");
                    break;

                case "UNIQUEIDENTIFIER":
                    ret = "Guid";
                    break;

                case "MONEY":
                case "DECIMAL":
                    ret = ("decimal");
                    break;

                case "DATE":
                case "DATETIME":
                case "SMALLDATETIME":
                case "TIME":
                    ret = ("DateTime");
                    break;
            }

            if (!nullable && ret != "string")
                ret += "?";

            return ret;
        }

    }
}
