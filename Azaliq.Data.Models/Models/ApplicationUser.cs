using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Azaliq.Data.Models.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
        }

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        public ICollection<ApplicationUserProduct> Favorites { get; set; } = new HashSet<ApplicationUserProduct>();
    }
}
