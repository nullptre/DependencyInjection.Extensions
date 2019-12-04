using System.Collections.Generic;

namespace Neleus.DependencyInjection.Extensions.Tests
{
    /// <summary>
    /// This client not aware of name and has no dependency on <see cref="IServiceByNameFactory"/>. The IEnumerable<int> dependency injected by IoC factory registration.
    /// </summary>
    public class ClientA
    {
        public IEnumerable<int> Dependency { get; private set; }

        public ClientA(IEnumerable<int> dependency)
        {
            Dependency = dependency;
        }
    }
}
