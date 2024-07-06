using Microsoft.EntityFrameworkCore;

namespace TiptapWebApi.Database
{
    public class DocumentDbContext : DbContext
    {
        public DocumentDbContext(DbContextOptions<DocumentDbContext> options) : base(options)
        {
        }
        public DbSet<Document> Documents { get; set; }
    }
}