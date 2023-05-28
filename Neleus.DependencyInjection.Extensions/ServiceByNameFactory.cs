using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
namespace Neleus.DependencyInjection.Extensions
{
    internal class ServiceByNameFactory<TService> : IServiceByNameFactory<TService>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<string, TypeLifetime> _registrations;

        public ServiceByNameFactory(IServiceProvider serviceProvider, IDictionary<string, TypeLifetime> registrations)
        {
            _serviceProvider = serviceProvider;
            _registrations = registrations;
        }

        public TService GetByName(string name)
        {
            if (!_registrations.TryGetValue(name, out var implementationType))
                throw new ArgumentException($"Service name '{name}' is not registered");
            switch (implementationType.Lifetime)
            {
                case Lifetime.Scoped:
                case Lifetime.Singleton:
                    if (implementationType.Instance == null)
                    {
                        implementationType.Instance = (TService)_serviceProvider.GetService(implementationType.Type);
                    }
                    return (TService)implementationType.Instance;
                default:
                    return (TService)_serviceProvider.GetService(implementationType.Type);
            }
        }

        public TService GetRequiredByName(string name)
        {
            if (!_registrations.TryGetValue(name, out var implementationType))
                throw new ArgumentException($"Service name '{name}' is not registered");
            switch (implementationType.Lifetime)
            {
                case Lifetime.Scoped:
                case Lifetime.Singleton:
                    if (implementationType.Instance == null)
                    {
                        implementationType.Instance = (TService)_serviceProvider.GetRequiredService(implementationType.Type);
                    }
                    return (TService)implementationType.Instance;
                default:
                    return (TService)_serviceProvider.GetRequiredService(implementationType.Type);
            }
        }

        public ICollection<string> GetNames()
        {
            return _registrations.Keys;
        }
    }
}
