using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ForumApi.DbContexts.Configuration;
using ForumAPI.Models;

namespace ForumApi.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid> 
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }



        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {

            string userIdentity = "SYSTEM";

            if (_httpContextAccessor.HttpContext != null)
            {
                userIdentity = _httpContextAccessor.HttpContext.User.Identity.Name;
            }

            var entries = ChangeTracker
                .Entries()
                .Where(e => (e.Entity is Entity) && (
                e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((Entity)entityEntry.Entity).ModificationDateTime = DateTime.Now;

                ((Entity)entityEntry.Entity).UserModified = userIdentity;

                if (entityEntry.State == EntityState.Added)
                {
                    ((Entity)entityEntry.Entity).UserCreated = userIdentity;
                    ((Entity)entityEntry.Entity).CreationDateTime = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.NoAction;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
