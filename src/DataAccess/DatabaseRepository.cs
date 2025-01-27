using Core.Entities;
using Npgsql;

namespace DataAccess
{
    public class DatabaseRepository
    {
        private readonly string _connectionString;

        public DatabaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateDatabase(Guid userId, string databaseName)
        {
            // 1. Create database in PostgreSQL
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Create actual database in PostgreSQL
                var createDbQuery = $"CREATE DATABASE \"{databaseName}\"";
                using (var command = new NpgsqlCommand(createDbQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // 2. Save metadata in our tracking table
                var query = "INSERT INTO databases (id, name, user_id, created_on) VALUES (@id, @name, @userId, @createdOn)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", Guid.NewGuid());
                    command.Parameters.AddWithValue("@name", databaseName);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@createdOn", DateTime.UtcNow);
                    command.ExecuteNonQuery();
                }
            }
        }


        public List<Database> GetUserDatabases(Guid userId)
        {
            var databases = new List<Database>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT id, name, user_id FROM databases WHERE user_id = @userId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            databases.Add(new Database
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                UserId = reader.GetGuid(2)
                            });
                        }
                    }
                }
            }
            return databases;
        }
    }
}
