using Dapper;
using FluentMigrator.Runner;
using Forum.Domain.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;

namespace Forum.Infrastructure.DataAccess.Migrations
{
    public class DatabaseMigration
    {
        public static void Migrate(DatabaseType databaseType, string connectionString, IServiceProvider serviceProvider)
        {
            if (databaseType == DatabaseType.SqlServer)
            {
                EnsureDatabaseCreated_SqlServer(connectionString);
            }
            else
            {
                EnsureDatabaseCreated_PostgreSql(connectionString);
            }

            MigrationDatabase(serviceProvider);
        }

        private static void EnsureDatabaseCreated_SqlServer(string connectionString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            var databaseName = connectionStringBuilder.InitialCatalog;

            connectionStringBuilder.Remove("Database");

            using var dbConnection = new SqlConnection(connectionStringBuilder.ConnectionString);

            var parameters = new DynamicParameters();
            parameters.Add("name", databaseName);

            var records = dbConnection.Query("SELECT * FROM sys.databases WHERE name = @name", parameters);

            if (!records.Any())
                dbConnection.Execute($"CREATE DATABASE {databaseName}");
        }

        private static void EnsureDatabaseCreated_PostgreSql(string connectionString)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);

            var databaseName = connectionStringBuilder.Database;

            connectionStringBuilder.Remove("Database");

            using var dbConnection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);

            var parameters = new DynamicParameters();
            parameters.Add("name", databaseName);

            var query = $"SELECT 1 FROM pg_database WHERE datname = @name";

            var records = dbConnection.Query(query, parameters).ToList();

            if (!records.Any())
            {
                dbConnection.Execute($"CREATE DATABASE \"{databaseName}\"");
            }
        }

        private static void MigrationDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.ListMigrations();

            runner.MigrateUp();
        }
    }
}
