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
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            // Define the primary key of the Category entity
            entity.HasKey(c => c.Id);

            // Define constraints for the Name column
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(256);

            // Define constraints for the Description column
            entity.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(1024);
        }
    }
}
