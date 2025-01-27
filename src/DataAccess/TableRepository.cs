using Core.DTOs;
using Core.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class TableRepository
    {
        private readonly string _connectionString;

        public TableRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateTable(Guid schemaId, string tableName)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // 1. Get database and schema names
                var query = @"
                    SELECT d.name as db_name, s.name as schema_name 
                    FROM schemas s 
                    JOIN databases d ON s.database_id = d.id 
                    WHERE s.id = @schemaId";

                string dbName, schemaName;
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@schemaId", schemaId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dbName = reader.GetString(0);
                            schemaName = reader.GetString(1);
                        }
                        else
                        {
                            throw new Exception("Schema not found.");
                        }
                    }
                }

                // 2. Create actual table in PostgreSQL
                using (var dbConnection = new NpgsqlConnection(_connectionString.Replace("Database=database_builder", $"Database={dbName}")))
                {
                    dbConnection.Open();
                    var createTableQuery = $"CREATE TABLE \"{schemaName}\".\"{tableName}\" (id SERIAL PRIMARY KEY)";

                    using (var command = new NpgsqlCommand(createTableQuery, dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                // 3. Save metadata
                var metadataQuery = "INSERT INTO tables (id, name, scheme_id, created_on) VALUES (@id, @name, @schemaId, @createdOn)";
                using (var command = new NpgsqlCommand(metadataQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", Guid.NewGuid());
                    command.Parameters.AddWithValue("@name", tableName);
                    command.Parameters.AddWithValue("@schemaId", schemaId);
                    command.Parameters.AddWithValue("@createdOn", DateTime.UtcNow);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Table> GetSchemaTables(Guid schemaId)
        {
            var tables = new List<Table>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // 1. Get database and schema information
                var schemaQuery = @"
                SELECT d.name as db_name, s.name as schema_name 
                FROM schemas s 
                JOIN databases d ON s.database_id = d.id 
                WHERE s.id = @schemaId";

                string dbName, schemaName;
                using (var cmd = new NpgsqlCommand(schemaQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@schemaId", schemaId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dbName = reader.GetString(0);
                            schemaName = reader.GetString(1);
                        }
                        else
                        {
                            return tables;
                        }
                    }
                }

                // 2. Get tables from metadata
                var query = "SELECT id, name, scheme_id, created_on FROM tables WHERE scheme_id = @schemaId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@schemaId", schemaId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(new Table
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                SchemeId = reader.GetGuid(2),
                                CreatedOn = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }
            return tables;
        }
    }
}
