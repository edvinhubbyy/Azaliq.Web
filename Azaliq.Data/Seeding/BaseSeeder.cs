using Azaliq.Data.Seeding.Interfaces;
using Azaliq.Data.Utilities.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;
using static Azaliq.Common.OutputMessages.ErrorMessages;


namespace Azaliq.Data.Seeding
{
    public abstract class BaseSeeder<T> : IBaseSeeder<T>
    {
        private readonly IValidator entityValidator;
        private readonly ILogger<T> logger;

        protected BaseSeeder(IValidator entityValidator, ILogger<T> logger)
        {
            this.FilePath = string.Empty;

            this.entityValidator = entityValidator;
            this.logger = logger;
        }

        public virtual string FilePath { get; }

        public IValidator EntityValidator
            => this.entityValidator;

        public ILogger<T> Logger
            => this.logger;

        public string BuildEntityValidatorWarningMessage(string entityName)
        {
            // Prepare log message with error messages from validation
            StringBuilder logMessage = new StringBuilder();
            logMessage.Append(string.Format(EntityImportError, entityName))
            .AppendLine(string.Join(Environment.NewLine, entityValidator.ErrorMessages));

            // Log the message
            return logMessage.ToString().TrimEnd();
        }

    }
}
