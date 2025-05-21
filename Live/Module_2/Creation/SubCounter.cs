namespace Creation;

internal class SubCounter : ICounter
{
    private int _counter = 0;

    public void Increment()
    {
        Console.WriteLine(--_counter);
    }
    public void Decrement()
    {
        Console.WriteLine(++_counter);
    }
}
