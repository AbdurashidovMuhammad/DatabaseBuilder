using Core.Entities;
using Npgsql;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class ColumnRepository
    {
        private readonly string _connectionString;

        public ColumnRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddColumn(Guid tableId, string columnName, string dataType, bool isNullable, bool isUnique)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"
                    SELECT d.name as db_name, s.name as schema_name, t.name as table_name
                    FROM tables t
                    JOIN schemas s ON t.scheme_id = s.id
                    JOIN databases d ON s.database_id = d.id
                    WHERE t.id = @tableId";

                string dbName, schemaName, tableName;
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@tableId", tableId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dbName = reader.GetString(0);
                            schemaName = reader.GetString(1);
                            tableName = reader.GetString(2);
                        }
                        else
                        {
                            throw new Exception("Table not found.");
                        }
                    }
                }

                using (var dbConnection = new NpgsqlConnection(_connectionString.Replace("Database=database_builder", $"Database={dbName}")))
                {
                    dbConnection.Open();
                    var alterTableQuery = $"ALTER TABLE \"{schemaName}\".\"{tableName}\" ADD COLUMN \"{columnName}\" {dataType} {(isNullable ? "" : "NOT NULL")} {(isUnique ? "UNIQUE" : "")}";

                    using (var command = new NpgsqlCommand(alterTableQuery, dbConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }

            CreateColumn(tableId, columnName, dataType, isNullable, isUnique);
        }

        private void CreateColumn(Guid tableId, string name, string dataType, bool isNullable, bool isUnique)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO columns (id, name, data_type, table_id, is_nullable, is_unique, created_on) VALUES (@id, @name, @dataType, @tableId, @isNullable, @isUnique, @createdOn)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", Guid.NewGuid());
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@dataType", dataType);
                    command.Parameters.AddWithValue("@tableId", tableId);
                    command.Parameters.AddWithValue("@isNullable", isNullable);
                    command.Parameters.AddWithValue("@isUnique", isUnique);
                    command.Parameters.AddWithValue("@createdOn", DateTime.UtcNow);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertRow(Guid tableId, Dictionary<string, object> columnValues)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // 1. Get database, schema, and table names
                var query = @"
            SELECT d.name as db_name, s.name as schema_name, t.name as table_name
            FROM tables t
            JOIN schemas s ON t.scheme_id = s.id
            JOIN databases d ON s.database_id = d.id
            WHERE t.id = @tableId";

                string dbName, schemaName, tableName;
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@tableId", tableId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dbName = reader.GetString(0);
                            schemaName = reader.GetString(1);
                            tableName = reader.GetString(2);
                        }
                        else
                        {
                            throw new Exception("Table not found.");
                        }
                    }
                }

                // 2. Build the INSERT query
                var columns = string.Join(", ", columnValues.Keys.Select(k => $"\"{k}\""));
                var values = string.Join(", ", columnValues.Values.Select(v => $"@{v}"));
                var insertQuery = $"INSERT INTO \"{schemaName}\".\"{tableName}\" ({columns}) VALUES ({values})";

                // 3. Execute the INSERT query
                using (var dbConnection = new NpgsqlConnection(_connectionString.Replace("Database=database_builder", $"Database={dbName}")))
                {
                    dbConnection.Open();
                    using (var command = new NpgsqlCommand(insertQuery, dbConnection))
                    {
                        foreach (var kvp in columnValues)
                        {
                            command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                        }
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<Column> GetTableColumns(Guid tableId)
        {
            var columns = new List<Column>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT id, name, data_type, is_nullable, is_unique FROM columns WHERE table_id = @tableId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tableId", tableId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columns.Add(new Column
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                DataType = reader.GetString(2),
                                IsNullable = reader.GetBoolean(3),
                                IsUnique = reader.GetBoolean(4),
                                TableId = tableId
                            });
                        }
                    }
                }
            }
            return columns;
        }
    }
}
