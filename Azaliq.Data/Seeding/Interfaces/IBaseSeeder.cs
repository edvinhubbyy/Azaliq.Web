using Azaliq.Data.Utilities.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azaliq.Data.Seeding.Interfaces
{
    public interface IBaseSeeder<T>
    {
        public string FilePath { get; }

        public IValidator EntityValidator { get; }

        public ILogger<T> Logger { get; }

        public string BuildEntityValidatorWarningMessage(string entityName);
    }
}
