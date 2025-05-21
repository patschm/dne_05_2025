

using Microsoft.Extensions.Configuration;

namespace Config;

internal class Program
{
    static void Main(string[] args)
    {
        //ViaCommandLineArguments(args);
        //ViaEnvironmentVariables();
        ViaConfig();

        Console.ReadLine();
    }

    private static void ViaConfig()
    {
        var bld = new ConfigurationBuilder();

        bld.AddJsonFile("appsettings.json", false, true);
        bld.AddUserSecrets<Program>();
        IConfiguration config = bld.Build();
        config.GetReloadToken().RegisterChangeCallback(obj=> Console.WriteLine($"nieuwe data {obj}"), config);


        string? naam = config.GetSection("Mijn:Voornaam").Value;
        Console.WriteLine(naam);

        MijnData data = new MijnData();
        config.GetSection("Mijn").Bind(data);

        Console.WriteLine($"{data.Voornaam} {data.Achternaam}");

        MijnData? data2 = config.GetSection("Mijn").Get<MijnData>();
        Console.WriteLine($"{data2.Voornaam} {data2.Achternaam}");

        Console.WriteLine(config.GetSection("Geheim:ConStr").Value);

        
    }

    private static void ViaEnvironmentVariables()
    {
        var collection = Environment.GetEnvironmentVariables();
        foreach (var item in collection)
        {
            Console.WriteLine($"{item}");
        }

        string? data = Environment.GetEnvironmentVariable("CONFIG_MIJNDATA");

        Console.WriteLine(data);

    }

    private static void ViaCommandLineArguments(string[] args)
    {
        string data = "";
        if (args.Length > 0)
        {
            data = args[0];
        }
        Console.WriteLine(data);
    }
}
