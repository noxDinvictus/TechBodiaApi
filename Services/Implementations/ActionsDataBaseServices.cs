using Microsoft.Data.SqlClient;
using TechBodiaApi.Services.Interfaces;

namespace TechBodiaApi.Services.Implementations
{
    public class ActionsDataBaseServices : IActionsDataBaseServices
    {
        private readonly IConfiguration _config;

        public ActionsDataBaseServices()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public async Task<string> BackupDatabase()
        {
            string dbName = "techbodia";
            string utcDate = DateTime.UtcNow.ToString("M-d-yyyy");
            string backupDirectory = @"D:\projects\db_back_up";
            string backupFilePath = $"{backupDirectory}\\{utcDate}-{dbName}.bak";

            Directory.CreateDirectory(backupDirectory);

            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            string backupCommandText = $@"
                    BACKUP DATABASE [{connection.Database}]
                    TO DISK = @BackupFilePath
                    WITH FORMAT, INIT, NAME = @BackupName, STATS = 10";

            using var command = new SqlCommand(backupCommandText, connection);
            command.Parameters.AddWithValue("@BackupFilePath", backupFilePath);
            command.Parameters.AddWithValue("@BackupName", connection.Database);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return $"Backup completed successfully. File saved at: {backupFilePath}";
        }

        public async Task<string> RestoreDatabase(string backupFilePath)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("OnflowDatabase"));
            string restoreCommandText = $@"
                    USE master;
                    ALTER DATABASE [{connection.Database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    RESTORE DATABASE [{connection.Database}]
                    FROM DISK = @BackupFilePath WITH REPLACE;
                    ALTER DATABASE [{connection.Database}] SET MULTI_USER;";

            using var command = new SqlCommand(restoreCommandText, connection);
            command.Parameters.AddWithValue("@BackupFilePath", backupFilePath);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return "Database restore completed successfully.";
        }
    }
}
