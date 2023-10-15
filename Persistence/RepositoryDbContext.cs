using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence;

public class RepositoryDbContext : DbContext
{    
    public RepositoryDbContext()
    {
    }

    public RepositoryDbContext(
        DbContextOptions<RepositoryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dog> Dogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);
}