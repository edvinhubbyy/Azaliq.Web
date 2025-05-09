using Azaliq.Data.Models.Models;
using Azaliq.Data.Seeding.Interfaces;
using Azaliq.Data.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Azaliq.Data.Dtos;
using static Azaliq.Common.OutputMessages.ErrorMessages;

namespace Azaliq.Data.Seeding
{
    public class ProductSeeder : BaseSeeder<ProductSeeder>, IEntitySeeder
    {

        private readonly AzaliqDbContext dbContext;
        private readonly ILogger<ProductSeeder> logger;
        private readonly IValidator entityValidator;

        public ProductSeeder(AzaliqDbContext dbContext,IValidator entityValidator,ILogger<ProductSeeder> logger)
            : base(entityValidator, logger)
        {
            this.dbContext = dbContext;

            this.logger = logger;
            this.entityValidator = entityValidator;
        }

        public override string FilePath
            => Path.Combine(AppContext.BaseDirectory, "Files", "Products.json");

        public async Task SeedEntityData()
        {
            await this.ImportProductsFromJson();
        }

        private async Task ImportProductsFromJson()
        {
            string ProductSeederStr = await File.ReadAllTextAsync(FilePath);

            try
            {
                ProductDto[]? ProductDtos = JsonSerializer.Deserialize<ProductDto[]>(ProductSeederStr);

                if (ProductDtos != null! && ProductDtos.Length > 0)
                {
                    ICollection<Product> validProducts = new List<Product>();

                    foreach (ProductDto ProductDto in ProductDtos)
                    {
                        // Validate the DTO using your existing validation logic
                        if (!this.EntityValidator.IsValid(ProductDto))
                        {
                            this.Logger.LogWarning(BuildEntityValidatorWarningMessage(nameof(Product)));
                            continue;
                        }

                        // Check if the referenced Category exists
                        Category? category = await dbContext
                            .Categories
                            .FirstOrDefaultAsync(c => c.Id == ProductDto.CategoryId);

                        if (category == null)
                        {
                            string logMessage = string.Format(EntityImportError, nameof(Product)) + ReferencedEntityMissing;
                            this.Logger.LogWarning(logMessage);
                            continue;
                        }

                        // Optionally: Check for duplicates (by ID or Name, depending on business rule)
                        Product? existingProduct = await dbContext
                            .Products
                            .FirstOrDefaultAsync(p => p.Id == ProductDto.Id || p.Name == ProductDto.Name);

                        if (existingProduct != null)
                        {
                            this.Logger.LogWarning(EntityInstanceAlreadyExist);
                            continue;
                        }

                        // Create and add the product
                        Product newProduct = new Product()
                        {
                            Id = ProductDto.Id,
                            Name = ProductDto.Name,
                            Price = ProductDto.Price,
                            Description = ProductDto.Description,
                            CategoryId = ProductDto.CategoryId
                        };

                        validProducts.Add(newProduct);
                    }

                    await dbContext.Products.AddRangeAsync(validProducts);
                    await dbContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message);
                throw;
            }

        }

    }
}
