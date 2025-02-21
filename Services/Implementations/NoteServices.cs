using Microsoft.EntityFrameworkCore;
using TechBodiaApi.Data;
using TechBodiaApi.Services.Interfaces;
using TechBodiaApi.Services.Models.DTO;
using DTO = TechBodiaApi.Data.Models.DTO.NoteDTO;
using Filter = TechBodiaApi.Data.Models.Filters.NoteFilter;
using Model = TechBodiaApi.Data.Models.Note;
using Payload = TechBodiaApi.Data.Models.Payload.NotePayload;

namespace TechBodiaApi.Services.Implementations
{
    public class NoteServices : INoteServices
    {
        private readonly TechBodiaContext _db;

        public NoteServices(TechBodiaContext db)
        {
            _db = db;
        }

        public async Task<DTO> Create(Payload payload, Guid userId)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var newItem = payload.ToDto().ToModel();
                newItem.CreatedByUserId = userId;

                _db.Notes.Add(newItem);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();
                return newItem.ToDTO();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"An error occurred while creating: {ex.Message}", ex);
            }
        }

        public DTO GetById(Guid id)
        {
            var ret = _db.Notes.FirstOrDefault(x => x.NoteId == id);

            if (ret == null)
            {
                throw new Exception("Item Does Not Exist");
            }
            return ret.ToDTO();
        }

        public ListResultDTO<Model, DTO, Filter> GetAllFiltered(Guid userId, Filter filter)
        {
            var ret = new ListResultDTO<Model, DTO, Filter>(filter);

            ret.BaseItems = _db.Notes.Where(x => x.CreatedByUserId == userId).AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                ret.BaseItems = ret.BaseItems.Where(x =>
                    x.Title.Contains(filter.SearchText)
                    || (x.Content != null && x.Content.Contains(filter.SearchText))
                );
            }

            ret.SetPageAndOrder();

            ret.Items = ret.BaseItems.ToList().Select(x => x.ToDTO()).ToList();

            return ret;
        }

        public async Task<DTO> Update(Payload payload, Guid Id)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var existing = await _db.Notes.FirstOrDefaultAsync(x => x.NoteId == Id);

                if (existing == null)
                {
                    throw new Exception("Item Does Not Exist");
                }

                var model = payload.ToDto().ToModel();
                model.CreatedAt = existing.CreatedAt;
                model.CreatedByUserId = existing.CreatedByUserId;
                model.NoteId = existing.NoteId;
                model.UpdatedAt = DateTime.UtcNow;

                _db.Entry(existing).State = EntityState.Detached;
                _db.Entry(model).State = EntityState.Modified;

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return model.ToDTO();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("An error occurred while updating. Error:", ex);
            }
        }

        public void Delete(Guid id)
        {
            var existing = _db.Notes.Find(id);

            if (existing == null)
            {
                throw new Exception("Item Does Not Exist");
            }

            _db.Notes.Remove(existing);
            _db.SaveChanges();
        }
    }
}
