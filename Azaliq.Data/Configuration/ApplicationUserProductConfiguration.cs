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
    internal class ApplicationUserProductConfiguration : IEntityTypeConfiguration<ApplicationUserProduct>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserProduct> builder)
        {
            builder.HasKey(x => new { x.ApplicationUserId, x.ProductId });

            builder.HasOne(x => x.ApplicationUser)
                .WithMany(x => x.Favorites)
                .HasForeignKey(x => x.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.FavoritedBy)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
