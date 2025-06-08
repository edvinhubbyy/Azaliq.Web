using Azaliq.Data.Dtos;
using Azaliq.Data.Models.Models;
using Azaliq.Data.Seeding.Interfaces;
using Azaliq.Data.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using static Azaliq.Common.OutputMessages.ErrorMessages;

namespace Azaliq.Data.Seeding
{
    public class ApplicationUserProductSeeder : BaseSeeder<ApplicationUserProductSeeder>, IEntitySeeder
    {
        private readonly AzaliqDbContext dbContext;
        private readonly ILogger<ApplicationUserProductSeeder> logger;
        private readonly IValidator entityValidator;

        public ApplicationUserProductSeeder(AzaliqDbContext dbContext, IValidator entityValidator, ILogger<ApplicationUserProductSeeder> logger)
            : base(entityValidator, logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.entityValidator = entityValidator;
        }

        public override string FilePath
            => Path.Combine(AppContext.BaseDirectory, "Files", "ApplicationUserProducts.json");

        public async Task SeedEntityData()
        {
            await ImportApplicationUserProductsFromJson();
        }

        private async Task ImportApplicationUserProductsFromJson()
        {
            string jsonStr = await File.ReadAllTextAsync(FilePath);

            
                try
                {
                    ApplicationUserProductDto[]? dtos = JsonSerializer.Deserialize<ApplicationUserProductDto[]>(jsonStr);

                    if (dtos != null && dtos.Length > 0)
                    {
                        ICollection<ApplicationUserProduct> validEntities = new List<ApplicationUserProduct>();

                        foreach (var dto in dtos)
                        {
                            if (!this.EntityValidator.IsValid(dto))
                            {
                                this.Logger.LogWarning(BuildEntityValidatorWarningMessage(nameof(ApplicationUserProduct)));
                                continue;
                            }

                            var userExists = await dbContext.Users.AnyAsync(u => u.Id == dto.ApplicationUserId);
                            var productExists = await dbContext.Products.AnyAsync(p => p.Id == dto.ProductId);

                            if (!userExists || !productExists)
                            {
                                this.Logger.LogWarning("User or Product does not exist for ApplicationUserProduct.");
                                continue;
                            }

                            var exists = await dbContext.ApplicationUserProducts
                                .AnyAsync(x => x.ApplicationUserId == dto.ApplicationUserId && x.ProductId == dto.ProductId);

                            if (exists)
                            {
                                this.Logger.LogWarning(EntityInstanceAlreadyExist);
                                continue;
                            }

                            var entity = new ApplicationUserProduct
                            {
                                ApplicationUserId = dto.ApplicationUserId,
                                ProductId = dto.ProductId
                            };

                            validEntities.Add(entity);
                        }

                        await dbContext.ApplicationUserProducts.AddRangeAsync(validEntities); 
                            await dbContext.SaveChangesAsync();
                        this.Logger.LogInformation("ApplicationUserProducts successfully seeded.");
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, "Error occurred while seeding ApplicationUserProducts.");
                    throw;
                }
            


        }
    }
}
