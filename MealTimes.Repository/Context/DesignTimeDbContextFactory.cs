using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using System.IO;


namespace MealTimes.Repository
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            // Used only by EF tooling at design time (migrations). Reads DATABASE_URL if set,
            // otherwise falls back to a local Postgres instance.
            var raw = Environment.GetEnvironmentVariable("DATABASE_URL")
                      ?? "Host=localhost;Port=5432;Database=MealTimes;Username=postgres;Password=postgres";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(AppDbContext.BuildNpgsqlConnectionString(raw));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
