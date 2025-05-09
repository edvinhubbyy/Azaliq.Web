using Azaliq.Data.Models.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azaliq.Data.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            // Define the primary key of the Order entity
            entity.HasKey(o => o.Id);

            // Define constraints for the OrderDate column
            entity.Property(o => o.OrderDate)
                .IsRequired()
                .HasColumnType("datetime2");

            // Define constraints for the TotalAmount column
            entity.Property(o => o.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
        }
    }

}
