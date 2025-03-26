using Npgsql;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LoginApp
{
    public class DatabaseMigration
    {
        private readonly string _connectionString;
        private const string MigrationTableName = "migrations";

        public DatabaseMigration(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InitializeDatabaseAsync()
        {
            // Создаем подключение к postgres для создания базы данных
            var postgresConnectionString = _connectionString.Replace("Database=logindb", "Database=postgres");
            using (var connection = new NpgsqlConnection(postgresConnectionString))
            {
                await connection.OpenAsync();
                
                // Проверяем существование базы данных
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT 1 FROM pg_database WHERE datname = 'logindb'";
                    var exists = await command.ExecuteScalarAsync() != null;

                    if (!exists)
                    {
                        // Создаем базу данных
                        command.CommandText = "CREATE DATABASE logindb";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }

            // Подключаемся к созданной базе данных
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Создаем таблицу для отслеживания миграций
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $@"
                        CREATE TABLE IF NOT EXISTS {MigrationTableName} (
                            id SERIAL PRIMARY KEY,
                            name VARCHAR(255) NOT NULL,
                            applied_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                        )";
                    await command.ExecuteNonQueryAsync();
                }

                // Проверяем, была ли уже применена начальная миграция
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"SELECT COUNT(*) FROM {MigrationTableName} WHERE name = 'initial'";
                    var count = Convert.ToInt32(await command.ExecuteScalarAsync());

                    if (count == 0)
                    {
                        // Читаем и выполняем SQL-скрипт
                        string sqlScript = await File.ReadAllTextAsync("create_database.sql");
                        using (var command2 = new NpgsqlCommand())
                        {
                            command2.Connection = connection;
                            command2.CommandText = sqlScript;
                            await command2.ExecuteNonQueryAsync();
                        }

                        // Отмечаем миграцию как выполненную
                        using (var command3 = new NpgsqlCommand())
                        {
                            command3.Connection = connection;
                            command3.CommandText = $"INSERT INTO {MigrationTableName} (name) VALUES ('initial')";
                            await command3.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }
    }
} 