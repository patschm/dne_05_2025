using System.Reflection;

namespace Attributen;


internal class Program
{
    static void Main(string[] args)
    {
        MyClass m1 =  new MyClass { Name = "Foo" };
        m1.DoeIets();

        Frameworkish(m1);

    }

    static void Frameworkish(IMyClass m)
    {
       var attrib =  m.GetType().GetInterface(nameof(IMyClass)).GetCustomAttribute<DemoAttribute>();
        if (attrib.IsValidAge(97))
        {
            m.DoeIets();
        }
        else
        {
            Console.WriteLine("Het is goed");
        }
    }
}


[Obsolete("Die gaat eraan. Gebruik MyClassV2", false)]
public class MyClass : IMyClass
{
    public string Name { get; set; }
    public void DoeIets()
    {
        Console.WriteLine($"Doet iets met {Name}");
    }
}


public class MyClassV2 : IMyClass
{
    public string Name { get; set; }
    public void DoeIets()
    {
        Console.WriteLine($"Doet iets V2-erigs met {Name}");
    }
}
