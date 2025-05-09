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
    public class CategoriesSeeder : BaseSeeder<CategoriesSeeder>, IEntitySeeder
    {

        private readonly AzaliqDbContext dbContext;
        private readonly ILogger<CategoriesSeeder> logger;
        private readonly IValidator entityValidator;

        public CategoriesSeeder(AzaliqDbContext dbContext, IValidator entityValidator, ILogger<CategoriesSeeder> logger) 
            : base(entityValidator, logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.entityValidator = entityValidator;
        }

        public override string FilePath
            => Path.Combine(AppContext.BaseDirectory, "Files", "Category.json");

        public async Task SeedEntityData()
        {
            await this.ImportCategoryFromJson();
        }

        private async Task ImportCategoryFromJson()
        {
            string CategoriesSeederStr = await File.ReadAllTextAsync(FilePath);

            try
            {

                CategoryDto[]? CategoryDtos = JsonSerializer.Deserialize<CategoryDto[]>(CategoriesSeederStr);

                if (CategoryDtos != null && CategoryDtos.Length > 0)
                {
                    ICollection<Category> validCategories = new List<Category>();

                    foreach (CategoryDto categoryDto in CategoryDtos)
                    {
                        // Validate the DTO using your existing validation logic
                        if (!this.EntityValidator.IsValid(categoryDto))
                        {
                            this.Logger.LogWarning(BuildEntityValidatorWarningMessage(nameof(Category)));
                            continue;
                        }

                        // Check for duplicates (by ID or Name, depending on your business rule)
                        Category? existingCategory = await dbContext
                            .Categories
                            .FirstOrDefaultAsync(c => c.Id == categoryDto.Id || c.Name == categoryDto.Name);

                        if (existingCategory != null)
                        {
                            this.Logger.LogWarning(EntityInstanceAlreadyExist);
                            continue;
                        }

                        // Create and add the category
                        Category newCategory = new Category()
                        {
                            Id = categoryDto.Id,
                            Name = categoryDto.Name,
                            Description = categoryDto.Description
                        };

                        validCategories.Add(newCategory);
                    }

                    // Insert valid categories into the database
                    await dbContext.Categories.AddRangeAsync(validCategories);
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
