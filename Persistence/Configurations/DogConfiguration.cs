using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class DogConfiguration : IEntityTypeConfiguration<Dog>
{
    public void Configure(EntityTypeBuilder<Dog> builder)
    {
        builder.ToTable("dogs");
        
        builder.HasKey(dog => dog.Name);
        builder.Property(dog => dog.Name).IsRequired();
        builder.Property(dog => dog.Name).HasMaxLength(50);

        builder.Property(dog => dog.Color).IsRequired();
        builder.Property(dog => dog.Color).HasMaxLength(50);

        builder.Property(dog => dog.TailLength).IsRequired();
        
        builder.Property(dog => dog.Color).IsRequired();
        
        // seed data
        builder.HasData(new List<Dog>
        {
            new() { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
            new() { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 },
            new() { Name = "Buddy", Color = "golden", TailLength = 18, Weight = 45 },
            new() { Name = "Luna", Color = "gray", TailLength = 12, Weight = 28 },
            new() { Name = "Rocky", Color = "brown", TailLength = 15, Weight = 36 },
            new() { Name = "Bella", Color = "white", TailLength = 10, Weight = 20 },
            new() { Name = "Max", Color = "spotted", TailLength = 14, Weight = 30 },
            new() { Name = "Coco", Color = "chocolate", TailLength = 20, Weight = 40 },
            new() { Name = "Milo", Color = "tan", TailLength = 9, Weight = 18 },
            new() { Name = "Daisy", Color = "fawn", TailLength = 16, Weight = 35 },
            new() { Name = "Bailey", Color = "sable", TailLength = 11, Weight = 25 },
        });

    }
}