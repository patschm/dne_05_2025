

using Microsoft.Extensions.DependencyInjection;

namespace Creation;

internal class Program
{
    static void Main(string[] args)
    {
        //OldSchool
        ViaDI();

    }

    private static void ViaDI()
    {
        var factory = new DefaultServiceProviderFactory();
        var services = new ServiceCollection();
        
        var bld = factory.CreateBuilder(services);
        bld.AddScoped<ICounter, SubCounter>();
        bld.AddTransient<IClient, CounterClient>();

        var provider = bld.BuildServiceProvider();

        ICounter cnt = provider.GetRequiredService<ICounter>();

        var scope = provider.CreateScope();
        //cnt = scope.ServiceProvider.GetRequiredService<ICounter>();
        // IClient client = new CounterClient(cnt);
        IClient client = scope.ServiceProvider.GetRequiredService<IClient>();
        client.DoeIets();
        scope.Dispose();

        // cnt = provider.GetRequiredService<ICounter>()

        var scope2 = provider.CreateScope();
        //cnt = scope2.ServiceProvider.GetRequiredService<ICounter>();
        //IClient client2 = new CounterClient(cnt);
        IClient client2 = scope2.ServiceProvider.GetRequiredService<IClient>();
        client2.DoeIets();

        // cnt = scope2.ServiceProvider.GetRequiredService<ICounter>();
        ///client2 = new CounterClient(cnt);
        client2 = scope2.ServiceProvider.GetRequiredService<IClient>();
        client2.DoeIets();
        scope2.Dispose();

    }

    private static void OldSchool()
    {
        ICounter cnt = Create();
        ICounter cnt2 = new SubCounter();
        IClient client = new CounterClient(cnt2);


        client.DoeIets();
    }

    static ICounter Create()
    {
        return new Counter();
    }
}
