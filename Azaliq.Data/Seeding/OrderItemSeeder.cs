using Azaliq.Data.Models.Models;
using Azaliq.Data.Seeding.Interfaces;
using Azaliq.Data.Utilities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Azaliq.Data.Dtos;
using static Azaliq.Common.OutputMessages.ErrorMessages;
using Order = Azaliq.Data.Models.Models.Order;

namespace Azaliq.Data.Seeding
{
    public class OrderItemSeeder : BaseSeeder<OrderItemSeeder>, IEntitySeeder
    {

        private readonly AzaliqDbContext dbContext;

        public OrderItemSeeder(AzaliqDbContext dbContext, ILogger<OrderItemSeeder> logger,
            IValidator entityValidator) : base(entityValidator, logger)
        {
            this.dbContext = dbContext;
        }

        public override string FilePath
            => Path.Combine(AppContext.BaseDirectory, "Files", "OrderItem.json");

        public async Task SeedEntityData()
        {
            await this.ImportCategoryFromJson();
        }

        private async Task ImportCategoryFromJson()
        {
            string OrderItemSeederStr = await File.ReadAllTextAsync(FilePath);

            try
            {

                OrderItemDto[]? OrderItemDtos = JsonSerializer.Deserialize<OrderItemDto[]>(OrderItemSeederStr);

                if (OrderItemDtos != null && OrderItemDtos.Length > 0)
                {
                    ICollection<OrderItem> validOrderItems = new List<OrderItem>();

                    foreach (OrderItemDto orderItemDto in OrderItemDtos)
                    {
                        // Validate the DTO using your existing validation logic
                        if (!this.EntityValidator.IsValid(orderItemDto))
                        {
                            this.Logger.LogWarning(BuildEntityValidatorWarningMessage(nameof(OrderItem)));
                            continue;
                        }

                        // Check if the referenced Order exists
                        Order? order = await dbContext
                            .Orders
                            .FirstOrDefaultAsync(o => o.Id == orderItemDto.OrderId);

                        if (order == null)
                        {
                            string logMessage = string.Format(EntityImportError, nameof(OrderItem)) + ReferencedEntityMissing;
                            this.Logger.LogWarning(logMessage);
                            continue;
                        }

                        // Check if the referenced Product exists
                        Product? product = await dbContext
                            .Products
                            .FirstOrDefaultAsync(p => p.Id == orderItemDto.ProductId);

                        if (product == null)
                        {
                            string logMessage = string.Format(EntityImportError, nameof(OrderItem)) + ReferencedEntityMissing;
                            this.Logger.LogWarning(logMessage);
                            continue;
                        }

                        // Optionally: Check for duplicates (by OrderId and ProductId, depending on your business rule)
                        OrderItem? existingOrderItem = await dbContext
                            .OrderItems
                            .FirstOrDefaultAsync(oi => oi.OrderId == orderItemDto.OrderId && oi.ProductId == orderItemDto.ProductId);

                        if (existingOrderItem != null)
                        {
                            this.Logger.LogWarning(EntityInstanceAlreadyExist);
                            continue;
                        }

                        // Create and add the order item
                        OrderItem newOrderItem = new OrderItem()
                        {
                            Id = orderItemDto.Id,
                            OrderId = orderItemDto.OrderId,
                            ProductId = orderItemDto.ProductId,
                            Quantity = orderItemDto.Quantity,
                            UnitPrice = orderItemDto.UnitPrice
                        };

                        validOrderItems.Add(newOrderItem);
                    }

                    // Insert valid order items into the database
                    await dbContext.OrderItems.AddRangeAsync(validOrderItems);
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
