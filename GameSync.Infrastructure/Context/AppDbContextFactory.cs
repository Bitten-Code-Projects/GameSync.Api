using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GameSync.Infrastructure.Context;

/// <summary>
/// A factory for creating instances of <see cref="AppDbContext"/> during design-time.
/// Required by EF Core CLI tools for generating migrations.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// Creates a new instance of <see cref="AppDbContext"/> with the required configuration for the database connection.
    /// </summary>
    /// <param name="args">Command-line arguments passed during design-time (not used).</param>
    /// <returns>A configured instance of <see cref="AppDbContext"/>.</returns>
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Retrieve the connection string from environment variables
        // or use a default value if the variable is not set.
        string connString = Environment.GetEnvironmentVariable("gameSyncConnString") ??
                            "server=127.0.0.1;database=GameSync;user=root;password=example;";

        // Configure the context to use MySQL with the detected server version
        optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));

        // Return the configured AppDbContext
        return new AppDbContext(optionsBuilder.Options);
    }
}