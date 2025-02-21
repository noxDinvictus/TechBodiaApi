using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace TechBodiaApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/actions-db")]
    [AllowAnonymous]
    public class ActionsDataBaseController : BaseController
    {
        private readonly IConfiguration _config;

        public ActionsDataBaseController()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        [HttpPost("backup")]
        public async Task<IActionResult> BackupDatabase()
        {
            string dbName = "techbodia";
            string utcDate = DateTime.UtcNow.ToString("M-d-yyyy");
            string backupDirectory = @"D:\projects\db_back_up";
            string backupFilePath = $"{backupDirectory}\\{utcDate}-{dbName}.bak";

            Directory.CreateDirectory(backupDirectory);

            try
            {
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

                return Ok(new { Message = "Backup completed", FilePath = backupFilePath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Backup failed", Error = ex.Message });
            }
        }

        [HttpPost("restore")]
        public async Task<IActionResult> RestoreDatabase(string backupFilePath)
        {
            if (!System.IO.File.Exists(backupFilePath))
            {
                return BadRequest(new { Message = "Backup file not found" });
            }

            try
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

                return Ok(new { Message = "Restore completed", FilePath = backupFilePath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Restore failed", Error = ex.Message });
            }
        }
    }
}
