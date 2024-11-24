using GameSync.Domain.GameSync.Entities;
using GameSync.Infrastructure.GameSync.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Infrastructure.GameSync;
public class GameSyncDbContext : DbContext
{
    public DbSet<LogEntity> Logs { get; set; }

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
        modelBuilder.ApplyConfiguration(new LogsConfiguration());
    }
}
