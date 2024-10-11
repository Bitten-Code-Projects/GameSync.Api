namespace GameSync.Infrastructure.Examples.Configurations;

using GameSync.Domain.Examples.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ExampleConfiguration : IEntityTypeConfiguration<Example>
{
    public void Configure(EntityTypeBuilder<Example> builder)
    {
        builder.HasKey(e => e.Id).HasName("id");

        builder.ToTable("example");

        builder.Property(e => e.Id)
            .UseMySqlIdentityColumn()
            .HasColumnName("id");

        builder.Property(e => e.Name)
            .HasMaxLength(64)
            .HasColumnName("name");

        builder.Property(e => e.Surname)
            .HasMaxLength(64)
        .HasColumnName("surname");

        builder.OwnsOne(e => e.Address)
            .Property(e => e.Street)
            .HasMaxLength(128)
            .HasColumnName("street");

        builder.OwnsOne(e => e.Address)
            .Property(e => e.City)
            .HasMaxLength(128)
            .HasColumnName("city");

        builder.OwnsOne(e => e.Address)
            .Property(e => e.HouseNumber)
            .HasMaxLength(64)
            .HasColumnName("housenumber");
    }
}
