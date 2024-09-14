namespace GameSync.Api.Infrastructure.Examples.Configurations;

using Microsoft.EntityFrameworkCore;
using GameSync.Api.Domain.Examples.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

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

        //modelBuilder.Entity<AccessLevel>().OwnsOne(x => x.Access);
        //builder.OwnsOne(x => x.Address);
        //builder.Property(c => c.Address)
        //    .HasConversion(c => c..Value, c => new Access(c.action, c.controller));

        //builder.HasNoKey();

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
