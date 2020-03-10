# DependencyInjection.Extensions
Provides name-based registrations to Microsoft.Extensions.DependencyInjection

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


