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

        private readonly IDictionary<string, TypeLifetime> _registrations;

        internal ServicesByNameBuilder(IServiceCollection services, NameBuilderSettings settings)
        {
            _services = services;
            _registrations = settings.CaseInsensitiveNames
                ? new Dictionary<string, TypeLifetime>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, TypeLifetime>();
        }

        /// <summary>
        /// Maps name to corresponding implementation.
        /// Note that this implementation has to be also registered in IoC container so
        /// that <see cref="IServiceByNameFactory&lt;TService&gt;"/> is be able to resolve it.
        /// </summary>
        public ServicesByNameBuilder<TService> Add(string name, Type implemtnationType)
        {
            var typeLifetime = new TypeLifetime() { Type = implemtnationType, Lifetime = Lifetime.Transient };
            _registrations.Add(name, typeLifetime);
            return this;
        }

        /// <summary>
        /// Generic version of <see cref="Add"/>
        /// </summary>
        public ServicesByNameBuilder<TService> AddScoped<TImplementation>(string name)
            where TImplementation : TService
        {
            return AddScoped(name, typeof(TImplementation));
        }

        /// <summary>
        /// Maps name to corresponding implementation.
        /// Note that this implementation has to be also registered in IoC container so
        /// that <see cref="IServiceByNameFactory&lt;TService&gt;"/> is be able to resolve it.
        /// </summary>
        public ServicesByNameBuilder<TService> AddScoped(string name, Type implemtnationType)
        {
            var typeLifetime = new TypeLifetime() { Type = implemtnationType, Lifetime = Lifetime.Scoped };
            _registrations.Add(name, typeLifetime);
            return this;
        }

        /// <summary>
        /// Generic version of <see cref="Add"/>
        /// </summary>
        public ServicesByNameBuilder<TService> AddSingleton<TImplementation>(string name)
            where TImplementation : TService
        {
            return AddSingleton(name, typeof(TImplementation));
        }

        /// <summary>
        /// Maps name to corresponding implementation.
        /// Note that this implementation has to be also registered in IoC container so
        /// that <see cref="IServiceByNameFactory&lt;TService&gt;"/> is be able to resolve it.
        /// </summary>
        public ServicesByNameBuilder<TService> AddSingleton(string name, Type implemtnationType)
        {
            var typeLifetime = new TypeLifetime() { Type = implemtnationType, Lifetime = Lifetime.Singleton };
            _registrations.Add(name, typeLifetime);
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
        public void BuildScoped()
        {
            var registrations = _registrations;
            //Registrations are shared across all instances
            _services.AddScoped<IServiceByNameFactory<TService>>(s => new ServiceByNameFactory<TService>(s, registrations));
        }
        public void BuildSingleton()
        {
            var registrations = _registrations;
            //Registrations are shared across all instances
            _services.AddSingleton<IServiceByNameFactory<TService>>(s => new ServiceByNameFactory<TService>(s, registrations));
        }
    }
}