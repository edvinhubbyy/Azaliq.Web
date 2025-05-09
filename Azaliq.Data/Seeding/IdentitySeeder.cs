using Azaliq.Data.Models.Models;
using Azaliq.Data.Seeding.Interfaces;
using Azaliq.Data.Utilities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static Azaliq.Common.OutputMessages.ErrorMessages;

namespace Azaliq.Data.Seeding
{
    public class IdentitySeeder : BaseSeeder<IdentitySeeder>, IEntitySeeder,IIdentitySeeder<ApplicationUser,IdentityRole<Guid>>
    {

        private readonly AzaliqDbContext dbContext;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;

        public IdentitySeeder(AzaliqDbContext dbContext,
            IValidator entityValidator,
            ILogger<IdentitySeeder> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager) 
            : base(entityValidator, logger)
        {
            this.dbContext = dbContext;

            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public UserManager<ApplicationUser> UserManager 
            => this.userManager;

        public RoleManager<IdentityRole<Guid>> RoleManager 
            => this.roleManager;



        public async Task SeedEntityData()
        {
            await this.SeedRoles();
            await this.SeedUsers();
        }


        private async Task SeedRoles()
        {
            string[] roles = { "Admin", "Manager", "User" };

            foreach (string role in roles)
            {
                bool roleExists = await this.RoleManager
                    .RoleExistsAsync(role);
                if (!roleExists)
                {
                    IdentityResult result = this.RoleManager
                        .CreateAsync(new IdentityRole<Guid>(role))
                        .GetAwaiter()
                        .GetResult();
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Format(FailedToCreateRole, role));
                    }
                }
            }
        }

        private async Task SeedUsers()
        {
            await this.SeedUser("admin@example.com", "Admin@123", "Admin");
            await this.SeedUser("appManager@example.com", "123asd", "Manager");
            await this.SeedUser("appUser@example.com", "123asd", "User");
        }

        private async Task SeedUser(string email, string password, string role)
        {
            ApplicationUser? user = await this.UserManager
                .FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                IdentityResult createUserResult = await this.UserManager
                    .CreateAsync(user, password);

                if (!createUserResult.Succeeded)
                {
                    throw new Exception(string.Format(FailedToCreateUser, email));
                }
            }

            bool isInRole = await this.UserManager
                .IsInRoleAsync(user, role);

            if (!isInRole)
            {
                IdentityResult addRoleResult = await this.UserManager
                    .AddToRoleAsync(user, role);

                if (!addRoleResult.Succeeded)
                {
                    throw new Exception(string.Format(FailedToAssignUserToRole, role, email));
                }
            }
        }


    }
}
