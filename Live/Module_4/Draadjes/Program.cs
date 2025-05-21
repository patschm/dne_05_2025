

using System.Diagnostics;

namespace Draadjes;

internal class Program
{
    static void Main(string[] args)
    {
        //Synchroon();
        //AsynchroonApm();
        //AsyncModern();
        //TaskChaining();
        //TerugNaarProbleem();
        //ThreadExplosion();
        //MetKrak();
        //ErgLangLopend();
        //Geinig();
        //OokGeinig();
        //HeelErgGeinigAsync();
        //Probleem();
        DeSemaphoor();
        //TreadDirector();

        Console.WriteLine("Einde Main");
        Console.ReadLine();
    }

    private static void DeSemaphoor()
    {
        ThreadPool.SetMinThreads(100, 100);
        Semaphore garare = new Semaphore(50, 50);

        Parallel.For(0, 100, idx => {
            Console.WriteLine($"Auto komt eraan {idx}");
            garare.WaitOne();
            Console.WriteLine($"Auto {idx} rijdt erin");
            Task.Delay(Random.Shared.Next(1000, 10000)).Wait();
            garare.Release();
            Console.WriteLine($"Auto {idx} rijdt eruit");

        });
    }

    static object stokje = new object();

    private static void Probleem()
    {
        int counter = 0;
        for (int i = 0; i < 10; i++) {

            Task.Run(() => {
                //Monitor.Enter(stokje);
                lock (stokje)
                {
                    int tmp = counter;
                    tmp++;
                    Task.Delay(500).Wait();
                    counter = tmp;
                    Console.WriteLine(counter);
                    //Monitor.Exit(stokje);
                }
            });
        }
    }

    private static void TreadDirector()
    {
        throw new NotImplementedException();
    }

    private static async void HeelErgGeinigAsync()
    {
        int a = 0;
        int b = 0;
        var zl1 = new AutoResetEvent(false);
        var zl2 = new AutoResetEvent(false);


        var t1 = Task.Run(() => { 
            Task.Delay(1000).Wait();
            a = 10;
            //zl1.Set();
        });

        var t2 = Task.Run(() => {
            Task.Delay(2000).Wait();
            b = 20;
            //zl2.Set();
        });


        //WaitHandle.WaitAny([zl1, zl2]);

        //Task.WaitAll(t1, t2);
        await Task.WhenAll(t1, t2);
        int result = a + b;
        Console.WriteLine(result);
    }

    private static async void OokGeinig()
    {
        try
        {
            await Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                throw new Exception("Ooops");
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static async void Geinig()
    {
        Task<int> t = Task.Run(() => LongAdd(1, 2));

        int result = await t;
        Console.WriteLine(result);
        Console.WriteLine("En daarna..");

        Task<int> t2 = Task.Run(() => LongAdd(3, 4));
        result = await t2;
        Console.WriteLine(result);
        Console.WriteLine("En weer verder...");

        result = await Task.Run(() => LongAdd(5, 6));
        Console.WriteLine(result);
        Console.WriteLine("En weer verder...");

        result = await LongAddAsync(7, 8);
        Console.WriteLine(result);
        Console.WriteLine("En weer verder...");

    }

    private static void ErgLangLopend()
    {
        CancellationTokenSource nikko = new CancellationTokenSource();
        CancellationToken bommetje = nikko.Token;
        Task.Run(() =>
        {
            for (int i = 0; i < 1000; i++)
            {
                //bommetje.IsCancellationRequested
                if (bommetje.IsCancellationRequested)
                {
                    return;
                }
                Console.WriteLine($"Iteratie {i}");
                Task.Delay(100).Wait();
            }
        });

        //Task.Delay(5000).Wait();
        //nikko.Cancel();
        nikko.CancelAfter(5000);
    }

    private static void MetKrak()
    {
        // Werkt niet
        //try
        //{
        //    Task.Run(() =>
        //    {
        //        Task.Delay(1000).Wait();
        //        throw new Exception("Ooops");
        //    });
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}

        Task.Run(() =>
            {
                Task.Delay(1000).Wait();
                throw new Exception("Ooops");
            }).ContinueWith(pt =>
            {
                if (pt.Exception != null)
                {
                    Console.WriteLine(pt.Exception?.InnerException?.Message);
                }

            });

    }

    private static void ThreadExplosion()
    {
        //ThreadPool.SetMinThreads(50, 50);
        Task[] tasks = new Task[50];
        Stopwatch sw = new Stopwatch();
        sw.Start();
        for (int i = 0; i < 50; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                Task.Delay(500).Wait();
                Console.WriteLine($"Task {i}");
            });
        }
        Task.WaitAll(tasks);
        sw.Stop();
        Console.WriteLine(sw.Elapsed);
    }

    private static void TerugNaarProbleem()
    {
        Task.Run(() => LongAdd(2, 3))
            .ContinueWith(pt => Console.WriteLine($"Het antwoord is {pt.Result}"));
    }

    private static void TaskChaining()
    {
        Task<int> t1 = Task.Run<int>(() =>
        {
            Console.WriteLine("Rekendereken");
            return 42;
        });
        t1.ContinueWith(pt => Console.WriteLine(pt.Result)).ContinueWith(pt => Console.WriteLine("Einde Chain"));
        t1.ContinueWith((pt) => Console.WriteLine("Parallel aan die andere"));
    }

    private static void AsyncModern()
    {
        Task<int> t1 = new Task<int>(() =>
        {
            int result = LongAdd(3, 4);
            return result;
        });
        t1.Start();

        Console.WriteLine($"Het antwoord is {t1.Result}");
    }

    private static void AsynchroonApm()
    {
        Func<int, int, int> del = LongAdd;

        //int result = del.Invoke(9, 4);
        IAsyncResult ar = del.BeginInvoke(5, 6, null, null);

        while (!ar.IsCompleted)
        {
            Console.Write(".");
            Task.Delay(100).Wait();
        }
        int result = del.EndInvoke(ar);
        Console.WriteLine($"Het antwoord is {result}");
    }

    private static void Synchroon()
    {
        int result = LongAdd(4, 5);
        Console.WriteLine($"Het antwoord is {result}");
    }

    static int LongAdd(int a, int b)
    {
        Task.Delay(5000).Wait();
        return a + b;
    }

    static Task<int> LongAddAsync(int a, int b)
    {
        return Task.Run(() => LongAdd(a, b));
    }
}
