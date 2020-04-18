using System;
using System.Data.SqlClient;

namespace MyCompany.NServiceBus.Core
{
    public static class SqlHelper
    {
        public static void ExecuteSql(string connectionString, string sql)
        {
            EnsureDatabaseExist(connectionString);
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using(var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void CreateSchema(string connectionString, string schema)
        {
            var sql = $@"if not exists (select *
                           from   sys.schemas
                           where  name = N'{schema}'
                           exec ('create schema {schema}');";
            ExecuteSql(connectionString, sql);
        }

        public static void EnsureDatabaseExist(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var database = builder.InitialCatalog;

            var masterConnection = connectionString.Replace(database, "master");

            using (var connection = new SqlConnection(masterConnection))
            {
                connection.Open();
                using(var command = connection.CreateCommand())
                {
                    command.CommandText = $@"if(db_id('{database}') is null) create database [{database}] ";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
