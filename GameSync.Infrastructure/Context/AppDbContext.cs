using GameSync.Infrastructure.Context.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Infrastructure.Context;

/// <summary>
/// Represents the Entity Framework Core database context for the application.
/// Inherits from <see cref="IdentityDbContext{TUser}"/> to integrate Identity features.
/// </summary>
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class using the specified options.
    /// </summary>
    /// <param name="options">Options used to configure the database context.</param>
    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    /// Configures the database context with the connection string during the application's configuration phase.
    /// This method is called when the context is being initialized, and it configures the <see cref="DbContextOptionsBuilder"/>
    /// to use MySQL as the database provider.
    /// </summary>
    /// <param name="optionsBuilder">The <see cref="DbContextOptionsBuilder"/> used to configure the context.</param>
    /// <remarks>
    /// This method checks whether the <see cref="DbContextOptionsBuilder"/> has already been configured.
    /// If not, it retrieves the connection string from an environment variable (named "GAMESYNC_CONNECTIONSTRING") or uses
    /// a default connection string if the environment variable is not set. The connection string is then used to configure
    /// the context to connect to a MySQL database.
    /// </remarks>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connString = Environment.GetEnvironmentVariable("GAMESYNC_CONNECTIONSTRING") ??
                                "server=127.0.0.1;database=GameSync;user=root;password=example;";

            optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));
        }
    }

    /// <summary>
    /// Configures the model for the context during model creation.
    /// Invokes the base <see cref="IdentityDbContext{TUser}.OnModelCreating"/> to apply Identity configurations.
    /// </summary>
    /// <param name="modelBuilder">
    /// The <see cref="ModelBuilder"/> used to define the shape of entity data and relationships in the database.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply default Identity configurations
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
            .Property(au => au.LastIP)
            .HasMaxLength(64)
            .HasColumnName("LastIP");

        // Add additional custom configurations here if needed
    }
}
