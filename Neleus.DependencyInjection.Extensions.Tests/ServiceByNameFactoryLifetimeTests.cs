using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Neleus.DependencyInjection.Extensions.Tests
{
    [TestClass]
    public class ServiceByNameFactoryLifetimeTests
    {
        private ServiceCollection _container;

        [TestInitialize]
        public void Init()
        {
            _container = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        }

        [TestMethod]
        public void ServiceByNameFactorySingleton_GetByName_ResolvesSingleton()
        {
            _container.AddTransient<List<int>>();

            _container.AddByName<IList<int>>()
                .AddSingleton<List<int>>("1")
                .AddSingleton<List<int>>("2")
                .BuildSingleton();

            var serviceProvider = _container.BuildServiceProvider();

            // direct resolution by calling GetByName
            var listA1 = serviceProvider.GetService<IServiceByNameFactory<IList<int>>>()
                .GetByName("1");
            var listA2 = serviceProvider.GetService<IServiceByNameFactory<IList<int>>>()
                .GetByName("2");
            listA1.Add(1);
            listA1.Add(2);
            listA2.Add(3);
            listA2.Add(4);

            var listB1 = serviceProvider.GetService<IServiceByNameFactory<IList<int>>>()
                .GetByName("1");
            var listB2 = serviceProvider.GetService<IServiceByNameFactory<IList<int>>>()
                .GetByName("2");

            Assert.AreSame(listA1, listB1);
            Assert.AreSame(listA2, listB2);
        }

        [TestMethod]
        public void ServiceByNameFactorySingleton_GetByName_ResolvesTransient()
        {
            _container.AddTransient<List<int>>();

            _container.AddByName<IList<int>>()
                .Add<List<int>>("1")
                .Add<List<int>>("2")
                .Build();

            var serviceProvider = _container.BuildServiceProvider();

            // direct resolution by calling GetByName
            var listA1 = serviceProvider.GetService<IServiceByNameFactory<IList<int>>>()
                .GetByName("1");
            var listA2 = serviceProvider.GetService<IServiceByNameFactory<IList<int>>>()
                .GetByName("2");
            listA1.Add(1);
            listA1.Add(2);
            listA2.Add(3);
            listA2.Add(4);

            var listB1 = serviceProvider.GetService<IServiceByNameFactory<IList<int>>>()
                .GetByName("1");
            var listB2 = serviceProvider.GetService<IServiceByNameFactory<IList<int>>>()
                .GetByName("2");

            Assert.AreNotEqual(listA1, listB1);
            Assert.AreNotEqual(listA2, listB2);
        }
    }
}
