# DependencyInjection.Extensions
Provides name-based registrations to Microsoft.Extensions.DependencyInjection

[Nuget link](https://www.nuget.org/packages/Neleus.DependencyInjection.Extensions)

See origin https://stackoverflow.com/a/39276306/2528649

## Usage example

In order to register your services you'll need to add following code to your Startup class:

        services.AddTransient<ServiceA>();
        services.AddTransient<ServiceB>();
        services.AddTransient<ServiceC>();
        services.AddByName<IService>()
            .Add<ServiceA>("key1")
            .Add<ServiceB>("key2")
            .Add<ServiceC>("key3")
            .Build();

Then you can resolve the appropriate service in two ways.

1) Injecting the dependency from factory registration.

        services.AddScoped<AccountController>(s => new AccountController(s.GetServiceByName<IService>("key2")));

In this case your client code is clean and has only dependency to `IService` and all the name resolution is performed on the factory expression.

2) Injecting dependency to IServiceByNameFactory interface:

        public AccountController(IServiceByNameFactory<IService> factory) {
		    var name = "key2"; // here you can have custom code of name resolution
            _service = factory.GetByName(name);
        }
In this case your client code depends on both IServiceByNameFactory and IService which can be heplful in case when client has its own logic of name resolution.

## Life Time Options

In order to register instances using Singleton or Scope you can use the following examples:

        services.AddTransient<ServiceA>();
        services.AddTransient<ServiceB>();
        services.AddTransient<ServiceC>();
        services.AddByName<IService>()
            .AddSingleton<ServiceA>("key1")
            .AddSingleton<ServiceB>("key2")
            .AddSingleton<ServiceC>("key3")
            .BuildSingleton();

## Loop through keys

There are times where you want to use the registered names as a way to handle do work against each key.

            _container.AddTransient<List<int>>();
            _container.AddTransient<HashSet<int>>();

            _container.AddByName<IEnumerable<int>>(new NameBuilderSettings()
            {
                CaseInsensitiveNames = true
            })
                .Add<List<int>>("list")
                .Add<HashSet<int>>("hashSet")
                .Build();

            var serviceProvider = _container.BuildServiceProvider();

            // allow to get each type
            var group = serviceProvider.GetService<IServiceByNameFactory<IEnumerable<int>>>();
            foreach (var name in group.GetNames())
            {
                var instance = group.GetByName(name);
                switch (name) {
                    case "list":
                        Assert.AreEqual(typeof(List<int>), instance.GetType());
                        break;
                    case "hashSet":
                        Assert.AreEqual(typeof(HashSet<int>), instance.GetType());
                        break;
                }
            }

## GetRequiredByName

Many times when sharing code between projects you want to throw an exception when a service isn't registered.  To do this here is an example:

            _container.AddTransient<HashSet<int>>();

            _container.AddByName<IEnumerable<int>>(new NameBuilderSettings()
            {
                CaseInsensitiveNames = true
            })
            .Add("list", typeof(List<int>))
            .Add("hashSet", typeof(HashSet<int>))
            .Build();


            var serviceProvider = _container.BuildServiceProvider();

            var serviceByNameFactory = serviceProvider.GetService<IServiceByNameFactory<IEnumerable<int>>>();

            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                var commandInstance = serviceByNameFactory.GetRequiredByName("list");
            });

