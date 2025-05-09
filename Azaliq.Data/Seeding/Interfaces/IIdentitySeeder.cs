using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azaliq.Data.Seeding.Interfaces
{
    public interface IIdentitySeeder<TUser,TRole> 
        where TUser : class, new()
        where TRole : class, new()
    {
        UserManager<TUser> UserManager { get; }

        RoleManager<TRole> RoleManager { get; }
    }
}
