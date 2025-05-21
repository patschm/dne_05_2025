using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InfraStructure;

public class ConsoleHost : IHostedService
{

    private readonly ICounter _counter;


    public ConsoleHost(ICounter counter)
    {
        _counter = counter;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
            for (int i = 0; i < 5; i++)
            {
                _counter.Increment();
                _counter.Show();
            }
            return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
