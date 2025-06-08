namespace Azaliq.Common.Constants
{
    public static class EntityConstants
    {
        // Decimal(18,2) is suitable SQL Server Type for money
        public const string MoneyType = "decimal(18,2)";
        public const string NoImageUrl = "no-image.jpeg";


        public static class ProductDto
        {
            /// <summary>
            /// Cinema name should be a text with length greater than or equal to 100
            /// </summary>
            public const int NameMaxLength = 100;

            public const int DescriptionMaxLength = 1024;
        }

        public static class Services
        {
            public const string ServiceCreatingError = "Something went wrong with the Service Creating, please check";

        }

    }
}
