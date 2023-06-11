using System;
using Dapper;
using Npgsql;
using System.Data;

namespace Pfm.Core.Helpers
{
    public class DapperHelper
    {
        private static IDbConnection PostgreSqlConnect()
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
            var conn = new NpgsqlConnection(AppSetting.ConnectionString);
            conn.Open();
            return conn;
        }
        public static IDbConnection PostgreSqlConnection
        {
            get
            {
                return PostgreSqlConnect();
            }
        }
    }
}

