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

Then you can use it via IServiceByNameFactory interface:

        public AccountController(IServiceByNameFactory<IService> factory) {
            _service = factory.GetByName("key2");
        }



