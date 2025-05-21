using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Runtime.CompilerServices;

namespace Loggen;

internal class Program
{
    static void Main(string[] args)
    {
        var bld = new ConfigurationBuilder();

        bld.SetBasePath(Directory.GetCurrentDirectory());
       bld.AddJsonFile("appsettings.json", false, true);
        IConfiguration config = bld.Build();

        var factory = LoggerFactory.Create(bld =>
        {
            bld.AddSimpleConsole();
            //bld.AddEventLog();
            //bld.AddFilter((cat, level) => {
               
            //    return cat == "Loggen.Program" && (int)level >= 1;
            //});
            bld.AddConfiguration(config.GetSection("Logging"));
        });

        var logger = factory.CreateLogger<Program>();
        WeLoggenWat(logger);

        var log2 = factory.CreateLogger<Dummy>();
        WeLoggenWat(log2);
    }

    static void WeLoggenWat<Y>(ILogger<Y> logger)
    {
        logger.LogDebug("Debug");
        logger.LogTrace("Trees");
        logger.LogInformation("Informatie");
        logger.LogWarning("Waarschuwing");
        logger.LogError("Fout");
        logger.LogCritical("Kritieke fout");
    }
}
