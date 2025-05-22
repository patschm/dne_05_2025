namespace Vullis;

internal class Program
{
    static Unmanaged um1 = new Unmanaged();
    static Unmanaged um2 = new Unmanaged();

    static void Main(string[] args)
    {
        try
        {
            um1.Open();

        }
        finally
        {
            um1.Dispose();
        }

        um1 = null;

       // GC.Collect();
       // GC.WaitForPendingFinalizers();

        using (um2)
        {
            um2.Open();
        }
        um2 = null;


        using(var um3 = new Unmanaged())
        {
            um3.Open();
        }

        Console.ReadLine();
        GC.Collect();
        GC.WaitForPendingFinalizers();

    }
}
