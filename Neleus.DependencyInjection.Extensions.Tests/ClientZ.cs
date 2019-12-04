using System.Collections.Generic;

namespace Neleus.DependencyInjection.Extensions.Tests
{
    /// <summary>
    /// This client is aware of the dependency name and uses it to get named service.
    /// </summary>
    public class ClientZ
    {
        public IEnumerable<int> Dependency { get; private set; }

        public ClientZ(IServiceByNameFactory<IEnumerable<int>> factory)
        {
            Dependency = factory.GetByName("hashSet");
        }
    }
}
