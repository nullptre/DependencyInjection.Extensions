using System;
using Microsoft.Extensions.DependencyInjection;

namespace Neleus.DependencyInjection.Extensions
{
    public static class FactoryServiceCollectionExtensions
    {
        /// <summary>
        /// Entry point for name-based registrations. This method should be called in order to start building
        /// named registrations for <typeparamref name="TService"/>"/>
        /// </summary>
        /// <returns><see cref="ServicesByNameBuilder&lt;TService&gt;"/> which is used to build multiple named registrations</returns>
        public static ServicesByNameBuilder<TService> AddByName<TService>(this IServiceCollection services)
        {
            return new ServicesByNameBuilder<TService>(services);
        }

        /// <summary>
        /// Provides instances of named registration. It is intended to be used in factory registrations, see example.
        /// </summary>
        /// <code>
        /// _container.AddTransient&lt;ClientA&gt;(s =&gt; new ClientA(s.GetByName&lt;IEnumerable&lt;int&gt;&gt;(&quot;list&quot;)));
        /// _container.AddTransient&lt;ClientB&gt;(s =&gt; new ClientB(s.GetByName&lt;IEnumerable&lt;int&gt;&gt;(&quot;hashSet&quot;)));
        /// </code>
        /// <returns></returns>
        public static TService GetByName<TService>(this IServiceProvider provider, string name)
        {
            var factory = provider.GetService<IServiceByNameFactory<TService>>();
            if (factory == null)
                throw new InvalidOperationException($"The factory {typeof(IServiceByNameFactory<TService>)} is not registered. Please use {nameof(FactoryServiceCollectionExtensions)}.{nameof(AddByName)}() to register names.");

            return factory.GetByName(name);
        }
    }
}