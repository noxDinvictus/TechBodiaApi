namespace TechBodiaApi.Services.Interfaces
{
    public interface IActionsDataBaseServices
    {
        Task<string> BackupDatabase();

        Task<string> RestoreDatabase(string backupFilePath);
    }
}
