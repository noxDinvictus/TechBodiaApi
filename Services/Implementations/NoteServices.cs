using Microsoft.EntityFrameworkCore;
using TechBodiaApi.Data;
using TechBodiaApi.Data.Models.DTO;
using TechBodiaApi.Services.Interfaces;
using DTO = TechBodiaApi.Data.Models.DTO.NoteDTO;
using Filter = TechBodiaApi.Data.Models.Filters.NoteFilter;
using Payload = TechBodiaApi.Data.Models.Payload.NotePayload;

namespace TechBodiaApi.Services.Implementations
{
    public class NoteServices : INoteServices
    {
        private readonly TechBodiaContext db;

        public NoteServices(TechBodiaContext db)
        {
            this.db = db;
        }
        public async Task<DTO> Create(Payload dto, Guid userId)
        {
            using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                var newItem = dto.ToDto().ToModel();
                newItem.CreatedByUserId = userId;

                db.Notes.Add(newItem);
                await db.SaveChangesAsync();

                await transaction.CommitAsync();
                return newItem.ToDto();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"An error occurred while creating: {ex.Message}", ex);
            }
        }

        public DTO GetById(Guid id)
        {
            var ret = db.Notes.FirstOrDefault(x => x.NoteId == id);

            if (ret == null)
            {
                throw new Exception("Item Does Not Exist");
            }

            return ret.ToDto();
        }

        public IEnumerable<NoteDTO> GetNotesList(Guid userId, Filter filter)
        {
            var query = db.Notes.Where(x => x.CreatedByUserId == userId);

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                query = query.Where(x => x.Title.Contains(filter.SearchText) || (x.Content != null && x.Content.Contains(filter.SearchText)));
            }

            return query.ToList().Select(x => x.ToDto());
        }


        public async Task<DTO> Update(Payload dto, Guid Id)
        {
            using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                var existing = await db.Notes.FirstOrDefaultAsync(x => x.NoteId == Id);

                if (existing == null)
                {
                    throw new Exception("Item Does Not Exist");
                }

                var model = dto.ToDto().ToModel();
                model.CreatedAt = existing.CreatedAt;
                model.CreatedByUserId = existing.CreatedByUserId;
                model.NoteId = existing.NoteId;
                model.UpdatedAt = DateTime.UtcNow;

                db.Entry(existing).State = EntityState.Detached;
                db.Entry(model).State = EntityState.Modified;

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return model.ToDto();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("An error occurred while updating. Error:", ex);
            }
        }

        public void Delete(Guid id)
        {
            var existing = db.Notes.Find(id);

            if (existing == null)
            {
                throw new Exception("Item Does Not Exist");
            }

            db.Notes.Remove(existing);
            db.SaveChanges();
        }
    }
}

