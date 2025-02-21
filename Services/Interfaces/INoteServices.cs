using TechBodiaApi.Services.Models.DTO;
using DTO = TechBodiaApi.Data.Models.DTO.NoteDTO;
using Filter = TechBodiaApi.Data.Models.Filters.NoteFilter;
using Model = TechBodiaApi.Data.Models.Note;
using Payload = TechBodiaApi.Data.Models.Payload.NotePayload;

namespace TechBodiaApi.Services.Interfaces
{
    public interface INoteServices
    {
        Task<DTO> Create(Payload payload, Guid userId);

        DTO GetById(Guid noteId);

        ListResultDTO<Model, DTO, Filter> GetAllFiltered(Guid userId, Filter filter);

        Task<DTO> Update(Payload payload, Guid Id);

        void Delete(Guid noteId);
    }
}
