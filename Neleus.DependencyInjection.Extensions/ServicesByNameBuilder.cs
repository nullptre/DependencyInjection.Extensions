using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Neleus.DependencyInjection.Extensions
{
    /// <summary>
    /// Provides easy fluent methods for building named registrations of the same interface
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class ServicesByNameBuilder<TService>
    {
        private readonly IServiceCollection _services;

        private readonly IDictionary<string, Type> _registrations;

        internal ServicesByNameBuilder(IServiceCollection services, NameBuilderSettings settings)
        {
            _services = services;
            _registrations = settings.CaseInsensitiveNames
                ? new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, Type>();
        }

        /// <summary>
        /// Maps name to corresponding implementation.
        /// Note that this implementation has to be also registered in IoC container so
        /// that <see cref="IServiceByNameFactory&lt;TService&gt;"/> is be able to resolve it.
        /// </summary>
        public ServicesByNameBuilder<TService> Add(string name, Type implemtnationType)
        {
            _registrations.Add(name, implemtnationType);
            return this;
        }

        /// <summary>
        /// Generic version of <see cref="Add"/>
        /// </summary>
        public ServicesByNameBuilder<TService> Add<TImplementation>(string name)
            where TImplementation : TService
        {
            return Add(name, typeof(TImplementation));
        }

        /// <summary>
        /// Adds <see cref="IServiceByNameFactory&lt;TService&gt;"/> to IoC container together with all registered implementations
        /// so it can be consumed by client code later. Note that each implementation has to be also registered in IoC container so
        /// <see cref="IServiceByNameFactory&lt;TService&gt;"/> is be able to resolve it from the container.
        /// </summary>
        public void Build()
        {
            var registrations = _registrations;
            //Registrations are shared across all instances
            _services.AddTransient<IServiceByNameFactory<TService>>(s => new ServiceByNameFactory<TService>(s, registrations));
        }
    }
}