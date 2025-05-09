using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azaliq.Data.Models.Models;

namespace Azaliq.Data.Configuration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            // Define the primary key of the Product entity
            entity.HasKey(p => p.Id);

            // Define constraints for the Name column
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(256);

            // Define constraints for the Description column
            entity.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(1024);

            // Define constraints for the Price column
            entity.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Define constraints for the StockQuantity column
            entity.Property(p => p.StockQuantity)
                .IsRequired();

            // Define relation between the Product and Category entities
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
