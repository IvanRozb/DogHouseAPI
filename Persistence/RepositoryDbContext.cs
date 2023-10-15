using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence;

public class RepositoryDbContext : DbContext
{    
    public RepositoryDbContext()
    {
    }

    public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dog> Dogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var relativePathToAppSettings = Path.Combine("..", "Web", "appsettings.json");
        var currentDirectory = Directory.GetCurrentDirectory();
        var appSettingsPath = Path.Combine(currentDirectory, relativePathToAppSettings);

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath, optional: false, reloadOnChange: true)
            .Build();
        
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    }
}