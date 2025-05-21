
namespace Creation;

internal class CounterClient : IClient
{
    private readonly ICounter _counter;

    public CounterClient(ICounter counter)
    {
        _counter = counter;
    }

    public void DoeIets()
    {
        _counter.Increment();
        _counter.Increment();
        _counter.Increment();
        _counter.Increment();
        _counter.Increment();
        _counter.Increment();
    }
}
