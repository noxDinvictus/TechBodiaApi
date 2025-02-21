using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBodiaApi.Services.Interfaces;

namespace TechBodiaApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/actions-db")]
    [AllowAnonymous]
    public class ActionsDataBaseController : BaseController
    {
        private readonly IActionsDataBaseServices _actionService;

        public ActionsDataBaseController(IActionsDataBaseServices actionService)
        {
            _actionService = actionService;
        }

        [HttpPost("backup")]
        public async Task<IActionResult> BackupDatabase()
        {
            try
            {
                var res = await _actionService.BackupDatabase();

                return Ok(new { Message = res });
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
                var res = await _actionService.RestoreDatabase(backupFilePath);

                return Ok(new { Message = res, FilePath = backupFilePath });
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
