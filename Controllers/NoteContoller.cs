using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBodiaApi.Attributes;
using TechBodiaApi.Services.Interfaces;
using DTO = TechBodiaApi.Data.Models.DTO.NoteDTO;
using Filter = TechBodiaApi.Data.Models.Filters.NoteFilter;
using Payload = TechBodiaApi.Data.Models.Payload.NotePayload;

namespace TechBodiaApi.Controllers
{
    [Authorize]
    [Route("/notes")]
    public class NoteContoller : BaseController
    {
        private readonly INoteServices noteService;

        public NoteContoller(INoteServices noteService)
        {
            this.noteService = noteService;
        }

        [ApiValidationFilter]
        [HttpPost]
        public async Task<ActionResult<ResultOk<DTO>>> Create([FromBody] Payload dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var res = await noteService.Create(dto, userId);
                return Success(res);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet("{id}")]
        public ActionResult<ResultOk<DTO>> GetById(Guid id)
        {
            try
            {
                var res = noteService.GetById(id);
                return Success(res);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost("list")]
        public ActionResult<ResultOk<IEnumerable<DTO>>> GetNotesList([FromBody] Filter filter)
        {
            try
            {
                var userId = GetCurrentUserId();
                var res = noteService.GetNotesList(userId, filter);
                return Success(res);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [ApiValidationFilter]
        [HttpPut("{id}")]
        public async Task<ActionResult<ResultOk<DTO>>> Update([FromRoute] Guid id, [FromBody] Payload dto)
        {
            try
            {
                var ret = await noteService.Update(dto, id);
                return Success(ret);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                noteService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}