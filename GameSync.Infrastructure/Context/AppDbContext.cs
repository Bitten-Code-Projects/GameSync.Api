using GameSync.Infrastructure.Examples.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Infrastructure.Context;

    public class AppDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string? connString = Environment.GetEnvironmentVariable("gameSyncConnString");

                // todo: move to appsettings?
                optionsBuilder.UseMySql(connString, ServerVersion.Parse("10.1.34-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ExampleConfiguration());
        }
    }
