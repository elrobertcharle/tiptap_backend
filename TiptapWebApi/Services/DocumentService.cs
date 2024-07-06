using DiffMatchPatch;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TiptapWebApi.Database;

namespace TiptapWebApi.Services
{
    public interface IDocumentService
    {
        Task<bool> ExistsAsync(int documentId);
        Task<int> CreateAsync();
        Task UpdateAsync(int documentId, List<Patch> patches);
        Task UpdateAsync(int documentId, string patches);
    }
    public class DocumentService : IDocumentService
    {
        private readonly DocumentDbContext _dbContext;
        public DocumentService(DocumentDbContext dbContext)
        {
            ArgumentNullException.ThrowIfNull(dbContext);
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(int documentId)
        {
            return await _dbContext.Documents.AnyAsync(d => d.Id == documentId);
        }

        public async Task<int> CreateAsync()
        {
            var document = new Database.Document
            {
                Content = ""
            };
            _dbContext.Add(document);
            await _dbContext.SaveChangesAsync();
            return document.Id;
        }

        public async Task UpdateAsync(int documentId, List<Patch> patches)
        {
            var document = await _dbContext.Documents.FindAsync(documentId);
            if (document == null)
                throw new InvalidOperationException($"The document with Id={documentId} does not exist.");
            var dmp = new diff_match_patch();
            dmp.patch_apply(patches, document.Content);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int documentId, string patches)
        {
            var dmp = new diff_match_patch();
            await UpdateAsync(documentId, dmp.patch_fromText(patches));
        }
    }
}
