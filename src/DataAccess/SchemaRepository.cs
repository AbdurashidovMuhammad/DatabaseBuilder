using Core.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class SchemaRepository
    {
        private readonly string _connectionString;

        public SchemaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateSchema(Guid databaseId, string schemaName)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // 1. Get database name from metadata
                string databaseName;
                using (var cmd = new NpgsqlCommand("SELECT name FROM databases WHERE id = @id", connection))
                {
                    cmd.Parameters.AddWithValue("@id", databaseId);
                    databaseName = (string)cmd.ExecuteScalar();
                }

                // 2. Create schema in the actual database
                using (var dbConnection = new NpgsqlConnection(_connectionString.Replace("Database=database_builder", $"Database={databaseName}")))
                {
                    dbConnection.Open();
                    using (var command = new NpgsqlCommand($"CREATE SCHEMA \"{schemaName}\"", dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                // 3. Save metadata
                var query = "INSERT INTO schemas (id, name, database_id, created_on) VALUES (@id, @name, @databaseId, @createdOn)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", Guid.NewGuid());
                    command.Parameters.AddWithValue("@name", schemaName);
                    command.Parameters.AddWithValue("@databaseId", databaseId);
                    command.Parameters.AddWithValue("@createdOn", DateTime.UtcNow);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Scheme> GetDatabaseSchemas(Guid databaseId)
        {
            var schemas = new List<Scheme>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT id, name, database_id FROM schemas WHERE database_id = @databaseId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@databaseId", databaseId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            schemas.Add(new Scheme
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                DatabaseId = reader.GetGuid(2)
                            });
                        }
                    }
                }
            }
            return schemas;
        }
    }
}
