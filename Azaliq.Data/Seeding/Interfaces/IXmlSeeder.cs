using Azaliq.Data.Utilities.Interfaces;

namespace Azaliq.Data.Seeding.Interfaces
{
    public interface IXmlSeeder
    {
        public string RootName { get; }

        public IXmlHelper XmlHelper { get; }
    }
}
