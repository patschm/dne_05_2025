//using SomeLibrary;

using System.Reflection;

namespace DeOnderwereld;

internal class Program
{
    static void Main(string[] args)
    {
       // Person p1 = new Person { Firstname = "Greet", LastName = "Kasteel", Age = 32 };
       // p1.Introduce();

       Assembly asm = Assembly.LoadFile(@"D:\DotnetEssentials\dne_05_2025\Live\Module_6\SomeLibrary.dll");
       Console.WriteLine(asm.FullName);

        //WatZitErin(asm);
        WatKanIkErmee(asm);

    }

    private static void WatKanIkErmee(Assembly asm)
    {
        Type? tPerson = asm.GetType("SomeLibrary.Person");
        Console.WriteLine(tPerson.FullName);
        Console.WriteLine(tPerson?.Name);

        PropertyInfo? pFirst = tPerson?.GetProperty("Firstname");
        PropertyInfo? pLast = tPerson?.GetProperty("LastName");
        PropertyInfo? pAge = tPerson?.GetProperty("Age");

        FieldInfo fAge = tPerson?.GetField("_age", BindingFlags.Instance | BindingFlags.NonPublic);

        MethodInfo? pIntro = tPerson?.GetMethod("Introduce");

        object? p1 = Activator.CreateInstance(tPerson);

        pFirst.SetValue(p1, "Henk");
        pLast.SetValue(p1, "Peters");
        pAge.SetValue(p1, 45);
        fAge.SetValue(p1, -45);
        pIntro.Invoke(p1, []);


        dynamic? p2 = Activator.CreateInstance(tPerson);
        p2.Firstname = "Marieke";
        p2.LastName = "Hendriks";
        p2.Age = 31;

        p2.Introduce();
        
    }

    private static void WatZitErin(Assembly asm)
    {
        foreach (var type in asm.GetTypes()) 
        {
            Console.WriteLine(type.FullName);
            foreach(var mem in type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                Console.WriteLine(mem.Name);
            }
            Console.WriteLine("===============================");
        }
    }
}
