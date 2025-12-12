
namespace ForumApi.Data
{
    public interface IUnitOfWork
    {
        // IDocumentRepository Documents { get; }


        /// <summary>
        /// Call SaveChangesAsync() in Entity Framework Core.
        /// </summary>
        /// <returns>Number of created entities in database.</returns>
        Task<int> Commit();
    }
}
