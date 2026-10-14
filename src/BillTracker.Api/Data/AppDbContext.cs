using Microsoft.EntityFrameworkCore;

namespace BillTracker.Api.Data;

// Domain classes stay free of EF attributes. Mapping lives in IEntityTypeConfiguration<T>
// classes next to each feature.
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
