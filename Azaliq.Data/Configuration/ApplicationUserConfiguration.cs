using Azaliq.Data.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azaliq.Data.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Configure the primary key (this is done by default when inheriting from IdentityUser)
            builder.HasKey(u => u.Id);

            // Configure the properties for UserName and Email (optional depending on your model)
            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            // ApplicationUserProduct: One ApplicationUser has many ApplicationUserProduct entries
            builder.HasMany(u => u.Favorites)
                .WithOne(fp => fp.ApplicationUser)  // One ApplicationUserProduct has one ApplicationUser
                .HasForeignKey(fp => fp.ApplicationUserId)  // The foreign key in ApplicationUserProduct
                .IsRequired();  // Ensures that every ApplicationUserProduct must have an ApplicationUser
        }
    }
}
