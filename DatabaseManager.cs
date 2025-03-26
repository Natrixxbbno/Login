using Npgsql;
using System;
using System.Threading.Tasks;

namespace LoginApp
{
    public class DatabaseManager
    {
        private readonly string _connectionString;
        private readonly DatabaseMigration _migration;

        public DatabaseManager()
        {
            // Используем стандартный пароль 'postgres' для пользователя 'postgres'
            _connectionString = "Host=localhost;Database=logindb;Username=postgres;Password=postgres";
            _migration = new DatabaseMigration(_connectionString);
        }

        public async Task InitializeAsync()
        {
            await _migration.InitializeDatabaseAsync();
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("password", password);

                    var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM users WHERE username = @username";
                    command.Parameters.AddWithValue("username", username);

                    try
                    {
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                    catch (PostgresException)
                    {
                        return false;
                    }
                }
            }
        }

        public async Task<bool> RegisterUserAsync(string username, string password, string email)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO users (username, password, email) VALUES (@username, @password, @email)";
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("password", password);
                    command.Parameters.AddWithValue("email", email);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return true;
                    }
                    catch (PostgresException)
                    {
                        return false;
                    }
                }
            }
        }
    }
} 