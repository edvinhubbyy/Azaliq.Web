using Azaliq.Data;
using Azaliq.Data.Models.Models;
using Azaliq.Data.Seeding;
using Azaliq.Data.Utilities.Interfaces;
using Azaliq.Data.Utilities;
using Azaliq.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connection string to your database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add DbContext for Entity Framework
builder.Services.AddDbContext<AzaliqDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IValidator, EntityValidator>();
builder.Services.AddSingleton<IXmlHelper, XmlHelper>();
builder.Services.AddScoped<IDbSeeder, ApplicationDbContextSeeder>();

// Add Identity services with IdentityUser<Guid> and Role
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 3;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
    .AddEntityFrameworkStores<AzaliqDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Ensure the seeding service is registered only once
builder.Services.AddScoped<IDbSeeder, ApplicationDbContextSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint(); // For migration in dev mode
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Custom error page in production
    app.UseHsts(); // Enforce secure connection
}

app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseStaticFiles(); // Serve static files (like CSS, JS)

app.UseRouting(); // Set up routing for controllers and Razor Pages

app.UseAuthentication(); // Enable Authentication (Login, Register)
app.UseAuthorization(); // Enable Authorization (Role-based access control)

// Map controllers and views (MVC)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor pages (Identity and other pages)
app.MapRazorPages();

// Run database seeding for development
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    try
    {
        IDbSeeder dataProcessor = services.GetRequiredService<IDbSeeder>();
        await dataProcessor.SeedData();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during database seeding: {ex.Message}");
        throw;
    }
}


app.Run();  // Run the application
