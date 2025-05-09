using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azaliq.Data.Seeding.Interfaces
{
    public interface IDbSeeder
    {
        Task SeedData();
    }
}
