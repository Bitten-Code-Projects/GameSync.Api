﻿using GameSync.Api.Domain.Examples.Entities;
using GameSync.Api.Infrastructure.Examples.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Infrastructure.Examples;

public class ExampleDbContext : DbContext
{
    public DbSet<Example> Example { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // todo: move to appsettings?
            optionsBuilder.UseMySql("server=localhost;port=3306;database=gamesync;uid=root", ServerVersion.Parse("10.1.34-mariadb"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ExampleConfiguration());
    }
}
