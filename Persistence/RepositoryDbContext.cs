using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence;

public sealed class RepositoryDbContext : DbContext
{    
    public RepositoryDbContext(
        DbContextOptions<RepositoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Dog> Dogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);
}