using DiffMatchPatch;
using Microsoft.EntityFrameworkCore;
using TiptapWebApi.Database;

namespace TiptapWebApi.Services
{
    public interface IDocumentService
    {
        Task<bool> ExistsAsync(int documentId);
        Task<int> CreateAsync(string content);
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

        public async Task<int> CreateAsync(string content)
        {
            var document = new Database.Document
            {
                Content = content
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
            document.Content = (string)dmp.patch_apply(patches, document.Content)[0];
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int documentId, string patches)
        {
            var dmp = new diff_match_patch();
            var s1 = "hello";
            var s2 = "hella";
            var diff = dmp.diff_main(s1, s2);
            dmp.diff_cleanupEfficiency(diff);
            var patch = dmp.patch_make(s1, diff);
            var content = (string)dmp.patch_apply(patch, s1)[0];
            var patchStr = dmp.patch_toText(patch);
            var patch2 = dmp.patch_fromText(patchStr);

            try
            {

                await UpdateAsync(documentId, dmp.patch_fromText(patches));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
