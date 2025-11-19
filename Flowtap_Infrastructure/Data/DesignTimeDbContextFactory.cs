using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Flowtap_Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Build configuration from appsettings.json
        // Try multiple possible locations for appsettings.json
        var basePath = Directory.GetCurrentDirectory();
        var possiblePaths = new[]
        {
            Path.Combine(basePath, "..", "Flowtap_Presentation"),
            Path.Combine(basePath, "..", "Flowtap_Configuration"),
            basePath
        };

        var configBuilder = new ConfigurationBuilder();
        bool found = false;
        foreach (var path in possiblePaths)
        {
            var appsettingsPath = Path.Combine(path, "appsettings.json");
            if (File.Exists(appsettingsPath))
            {
                configBuilder.SetBasePath(path)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
                found = true;
                break;
            }
        }

        if (!found)
        {
            // Fallback to Flowtap_Presentation
            configBuilder.SetBasePath(Path.Combine(basePath, "..", "Flowtap_Presentation"))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
        }

        var configuration = configBuilder.Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("postgres", StringComparison.OrdinalIgnoreCase))
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
        else
        {
            // Fallback to in-memory for development
            optionsBuilder.UseInMemoryDatabase("FlowtapDb");
        }

        return new AppDbContext(optionsBuilder.Options);
    }
}

