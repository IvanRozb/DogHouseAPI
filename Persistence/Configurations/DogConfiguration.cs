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

        builder.Property(dog => dog.Name).ValueGeneratedOnAdd();

        builder.Property(dog => dog.Name).HasMaxLength(50);
        builder.Property(dog => dog.Name).IsRequired();
        
        builder.Property(dog => dog.Color).HasMaxLength(50);
        builder.Property(dog => dog.Color).IsRequired();
        
        builder.Property(dog => dog.TailLength).IsRequired();
        
        builder.Property(dog => dog.Weight).IsRequired();
        
    }
}