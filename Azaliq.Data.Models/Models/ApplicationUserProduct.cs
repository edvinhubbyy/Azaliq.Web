namespace Azaliq.Data.Models.Models;

public class ApplicationUserProduct
{
    public Guid ApplicationUserId { get; set; }  // Foreign key to ApplicationUser
    public ApplicationUser ApplicationUser { get; set; } = null!;  // Navigation property to ApplicationUser

    public Guid ProductId { get; set; }   // Assuming a foreign key to Product
    public Product Product { get; set; } = null!;  // Navigation property to Product
}