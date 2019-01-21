using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using DatabaseSchemaAdvanced.Model;
using DatabaseSchemaReader;
using Npgsql;

namespace DatabaseSchemaAdvanced.Service
{
    public class DataService : IDataService
    {
        public void ExportSchemaToCsv(List<NodeItem> columns, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(columns);
                    csv.Flush();
                }
            }
        }

        public List<NodeItem> ImportSchemaFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader))
                {
                    //return new List<NodeItem>(csv.GetRecords<NodeItem>());
                    var records = new List<NodeItem>();
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var record = new NodeItem(NodeType.Column)
                        {
                            Table = csv.GetField("Table"),
                            Name = csv.GetField("Column"),
                            Description = csv.GetField("Description"),
                        };
                        records.Add(record);
                    }
                    return records;
                }
            }
        }

        public void SetColumnsDescriptionProperties(string connectionString, List<NodeItem> columns, string providerType)
        {
            switch(providerType)
            {
                case DbProvider.SQLSERVER:
                    SetColumnDescriptionForSqlServer(connectionString, columns);
                    return;
                case DbProvider.POSTGRESQL:
                    SetColumnDescriptionForPostgreSql(connectionString, columns);
                    return;
                default:
                    throw new Exception("Provider not supported");
            }
        }

        private void SetColumnDescriptionForPostgreSql(string connectionString, List<NodeItem> columns)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                //connection.BeginTransaction();
                foreach (NodeItem item in columns)
                {
                    //string schema = item.Table.Split('.')[0];
                    string tableName = item.Table.Split('.')[1];

                    NpgsqlCommand sqlCommand = connection.CreateCommand();
                    if(!String.IsNullOrEmpty(item.Description))
                        sqlCommand.CommandText = String.Format("COMMENT ON COLUMN {0}.{1} IS '{2}'", tableName, item.Name, item.Description);
                    else
                        sqlCommand.CommandText = String.Format("COMMENT ON COLUMN {0}.{1} IS NULL", tableName, item.Name);
                    sqlCommand.CommandType = CommandType.Text;

                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    catch (Exception oex)
                    {
                        //retValue = true;
                        //Logging.Logger.append("ERREUR: " + oex.Message, Logging.Logger.FATAL);
                        throw oex;
                    }
                }
                connection.Close();
            }
        }

        private void SetColumnDescriptionForSqlServer(string connectionString, List<NodeItem> columns)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //connection.BeginTransaction();
                foreach (NodeItem item in columns)
                {
                    string schema = item.Table.Split('.')[0];
                    string tableName = item.Table.Split('.')[1];

                    SqlCommand sqlCommand = connection.CreateCommand();
                    sqlCommand.CommandText = "SetColumnDescription";
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    SqlParameter parSchemaName = new SqlParameter("@schemaName".ToLower(), SqlDbType.VarChar);
                    parSchemaName.Direction = ParameterDirection.Input;
                    parSchemaName.Value = schema;
                    sqlCommand.Parameters.Add(parSchemaName);

                    SqlParameter parTableName = new SqlParameter("@tableName".ToLower(), SqlDbType.VarChar);
                    parTableName.Direction = ParameterDirection.Input;
                    parTableName.Value = tableName;
                    sqlCommand.Parameters.Add(parTableName);

                    SqlParameter parColumnName = new SqlParameter("@columnName".ToLower(), SqlDbType.VarChar);
                    parColumnName.Direction = ParameterDirection.Input;
                    parColumnName.Value = item.Name;
                    sqlCommand.Parameters.Add(parColumnName);

                    SqlParameter parDescription = new SqlParameter("@description".ToLower(), SqlDbType.VarChar);
                    parDescription.Direction = ParameterDirection.Input;
                    parDescription.Value = item.Description;
                    sqlCommand.Parameters.Add(parDescription);

                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                    }

                    catch (Exception oex)
                    {
                        //retValue = true;
                        //Logging.Logger.append("ERREUR: " + oex.Message, Logging.Logger.FATAL);
                        throw oex;
                    }
                }
                connection.Close();
            }
        }
    }
}
