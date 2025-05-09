using Azaliq.Data.Seeding.Interfaces;
using Microsoft.Extensions.Logging;

namespace Azaliq.Data.Seeding
{
    public class DbSeeder : IDbSeeder
    {
        private readonly IEnumerable<IEntitySeeder> seeders;
        private readonly ILogger<DbSeeder> logger;

        public DbSeeder(IEnumerable<IEntitySeeder> seeders, ILogger<DbSeeder> logger)
        {
            this.seeders = seeders;
            this.logger = logger;
        }

        public async Task SeedData()
        {
            foreach (var seeder in seeders)
            {
                try
                {
                    await seeder.SeedEntityData();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error seeding data with {seeder.GetType().Name}");
                }
            }
        }
    }
}
