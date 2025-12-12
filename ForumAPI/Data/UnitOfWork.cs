

using ForumApi.DbContext;
using ForumApi.Data;

namespace ForumApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        // private DocumentRepository _documentRepository;
        

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        // public IDocumentRepository Documents => _documentRepository = _documentRepository ?? new DocumentRepository(_context);
       

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
