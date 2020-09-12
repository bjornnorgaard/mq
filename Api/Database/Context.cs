using Microsoft.EntityFrameworkCore;

namespace Api.Database
{
    public class Context : DbContext
    {
        public DbSet<CachedItem> CachedItems { get; set; }

        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}