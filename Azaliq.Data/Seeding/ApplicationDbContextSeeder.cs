using Azaliq.Data;
using Azaliq.Data.Models.Models;
using Azaliq.Data.Seeding;
using Azaliq.Data.Seeding.Interfaces;
using Azaliq.Data.Utilities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

public class ApplicationDbContextSeeder : IDbSeeder
{

    private readonly AzaliqDbContext dbContext;

    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole<Guid>> roleManager;

    private readonly IValidator entityValidator;
    private readonly IXmlHelper xmlHelper;

    private readonly ILogger<ApplicationDbContextSeeder> logger;

    private readonly ICollection<IEntitySeeder> entitySeeders;

    public ApplicationDbContextSeeder(AzaliqDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IValidator entityValidator,
        IXmlHelper xmlHelper,
        ILogger<IdentitySeeder> identityLogger,
        ILogger<CategoriesSeeder> categoryLogger,
        ILogger<ProductSeeder> productLogger,
        ILogger<ApplicationUserProductSeeder> applicationUserProductLogger, // Renamed for consistency
        ILogger<OrderItemSeeder> orderItemLogger,
        ILogger<OrderSeeder> orderLogger  
        )
    {
        this.dbContext = dbContext;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.entityValidator = entityValidator;
        this.xmlHelper = xmlHelper;

        this.entitySeeders = new List<IEntitySeeder>();
        this.InitializeDbSeeders(identityLogger,categoryLogger, productLogger, applicationUserProductLogger, orderItemLogger, orderLogger);
    }

    public async Task SeedData()
    {
        foreach (IEntitySeeder entitySeeder in this.entitySeeders)
        {
            await entitySeeder.SeedEntityData();
        }
    }





    private void InitializeDbSeeders(
    ILogger<IdentitySeeder> identityLogger,
    ILogger<CategoriesSeeder> categorieLogger,
    ILogger<ProductSeeder> productLogger,
    ILogger<ApplicationUserProductSeeder> applicationUserProductLogger,
    ILogger<OrderItemSeeder> orderItemLogger,
    ILogger<OrderSeeder> orderLogger)
    {
        this.entitySeeders.Add(new IdentitySeeder(this.dbContext, this.entityValidator, identityLogger, this.userManager, this.roleManager));

        // Order matters: Categories BEFORE Products
        this.entitySeeders.Add(new CategoriesSeeder(this.dbContext, this.entityValidator, categorieLogger));
        this.entitySeeders.Add(new ProductSeeder(this.dbContext, this.entityValidator, productLogger));
        this.entitySeeders.Add(new ApplicationUserProductSeeder(this.dbContext,this.entityValidator, applicationUserProductLogger));

        // Add other seeders after their dependencies
        this.entitySeeders.Add(new OrderItemSeeder(this.dbContext, orderItemLogger,this.entityValidator));
        this.entitySeeders.Add(new OrderSeeder(this.dbContext, orderLogger, this.entityValidator));
    }

}