using Microsoft.AspNetCore.Mvc;
using TechBodiaApi.Api.Attributes;
using TechBodiaApi.Attributes;
using TechBodiaApi.Data.Definitions;
using TechBodiaApi.Services.Interfaces;
using TechBodiaApi.Services.Models.DTO;

using DTO = TechBodiaApi.Data.Models.DTO.NoteDTO;
using Filter = TechBodiaApi.Data.Models.Filters.NoteFilter;
using Model = TechBodiaApi.Data.Models.Note;
using Payload = TechBodiaApi.Data.Models.Payload.NotePayload;

namespace TechBodiaApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/notes")]
    [Roles(Roles.User)]
    public class NoteController : BaseController
    {
        private readonly INoteServices _noteService;

        public NoteController(INoteServices noteService)
        {
            _noteService = noteService;
        }

        [ApiValidationFilter]
        [HttpPost]
        public async Task<ActionResult<ResultOk<DTO>>> Create([FromBody] Payload payload)
        {
            try
            {
                var userId = GetCurrentUserId();
                var res = await _noteService.Create(payload, userId);
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
                var res = _noteService.GetById(id);
                return Success(res);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet("list")]
        public ActionResult<ResultOk<ListResultDTO<Model, DTO, Filter>>> GetAllFiltered(
            [FromQuery] Filter filter
        )
        {
            try
            {
                var userId = GetCurrentUserId();
                var res = _noteService.GetAllFiltered(userId, filter);
                return Success(res);
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [ApiValidationFilter]
        [HttpPut("{id}")]
        public async Task<ActionResult<ResultOk<DTO>>> Update(
            [FromRoute] Guid id,
            [FromBody] Payload payload
        )
        {
            try
            {
                var ret = await _noteService.Update(payload, id);
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
                _noteService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
