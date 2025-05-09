using Azaliq.Data.Models.Models;
using Azaliq.Data.Seeding.Interfaces;
using Azaliq.Data.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Azaliq.Data.Dtos;
using Azaliq.Data.Models;
using static Azaliq.Common.OutputMessages.ErrorMessages;

namespace Azaliq.Data.Seeding
{
    public class OrderSeeder : BaseSeeder<OrderSeeder>, IEntitySeeder
    {

        private readonly AzaliqDbContext dbContext;
        private readonly ILogger<OrderSeeder> logger;
        private readonly IValidator entityValidator;

        public OrderSeeder(AzaliqDbContext dbContext,ILogger<OrderSeeder> logger,IValidator entityValidator)
            : base(entityValidator, logger)
        {
            this.dbContext = dbContext;

            this.logger = logger;
            this.entityValidator = entityValidator;
        }

        public override string FilePath
            => Path.Combine(AppContext.BaseDirectory, "Files", "Orders.json");

        public async Task SeedEntityData()
        {
            await this.ImportCategoryFromJson();
        }

        private async Task ImportCategoryFromJson()
        {
            string OrderSeederStr = await File.ReadAllTextAsync(FilePath);

            try
            {

                OrderDto[]? OrderDtos = JsonSerializer.Deserialize<OrderDto[]>(OrderSeederStr);

                if (OrderDtos != null! && OrderDtos.Length > 0)
                {
                    ICollection<Order> validOrders = new List<Order>();

                    foreach (OrderDto orderDto in OrderDtos)
                    {
                        // Validate the DTO using your existing validation logic
                        if (!this.EntityValidator.IsValid(orderDto))
                        {
                            this.Logger.LogWarning(BuildEntityValidatorWarningMessage(nameof(Order)));
                            continue;
                        }

                        // Check if the referenced Customer exists
                        Customer? customer = await dbContext
                            .Customers
                            .FirstOrDefaultAsync(c => c.Id == orderDto.CustomerId);

                        if (customer == null)
                        {
                            string logMessage = string.Format(EntityImportError, nameof(Order)) + ReferencedEntityMissing;
                            this.Logger.LogWarning(logMessage);
                            continue;
                        }

                        // Check if the referenced ApplicationUser exists
                        ApplicationUser? user = await dbContext
                            .ApplicationUsers
                            .FirstOrDefaultAsync(au => au.Id == orderDto.ApplicationUserId);

                        if (user == null)
                        {
                            string logMessage = string.Format(EntityImportError, nameof(Order)) + ReferencedEntityMissing;
                            this.Logger.LogWarning(logMessage);
                            continue;
                        }

                        // Optionally: Check for duplicates (by ID, depending on your business rule)
                        Order? existingOrder = await dbContext
                            .Orders
                            .FirstOrDefaultAsync(o => o.Id == orderDto.Id);

                        if (existingOrder != null)
                        {
                            this.Logger.LogWarning(EntityInstanceAlreadyExist);
                            continue;
                        }

                        // Create and add the order
                        Order newOrder = new Order()
                        {
                            Id = orderDto.Id,
                            CustomerId = orderDto.CustomerId,
                            OrderDate = orderDto.OrderDate,
                            TotalAmount = orderDto.TotalAmount,
                            ApplicationUserId = orderDto.ApplicationUserId
                        };

                        validOrders.Add(newOrder);
                    }

                    await dbContext.Orders.AddRangeAsync(validOrders);
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
