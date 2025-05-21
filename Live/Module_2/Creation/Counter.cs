namespace Creation;

internal class Counter : ICounter
{
    private int _counter = 0;

    public void Increment()
    {
        Console.WriteLine(++_counter);
    }
    public void Decrement()
    {
        Console.WriteLine(--_counter);
    }
}
