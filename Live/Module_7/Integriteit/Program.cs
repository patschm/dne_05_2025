
using System.Security.Cryptography;
using System.Text;

namespace Integriteit;

internal class Program
{
    static void Main(string[] args)
    {
        //TestHash();
        // TestHashMAC();
        TestHashAsymm();
    }

    private static void TestHashAsymm()
    {
        // Zender
        string msg = "Hello World";
        SHA384 alg = SHA384.Create();
        byte[] hash = alg.ComputeHash(Encoding.UTF8.GetBytes(msg));
        DSA dsa = DSA.Create();
        string pubKey = dsa.ToXmlString(false);
        byte[] signature = dsa.SignData(hash, HashAlgorithmName.SHA384);

        Console.WriteLine(Convert.ToBase64String(signature));


        // Ed
        //msg += ".";


        // Ontvanger
        SHA384 alg2 = SHA384.Create();
        DSA dsa2 = DSA.Create();
        dsa2.FromXmlString(pubKey);
        byte[] hash2 = alg2.ComputeHash(Encoding.UTF8.GetBytes(msg));
        bool isOk = dsa2.VerifyData(hash2, signature, HashAlgorithmName.SHA384);
        Console.WriteLine(isOk ? "Origineel": "Aan geklooid");

        //Console.WriteLine(Convert.ToBase64String(hash2));
    }

    private static void TestHashMAC()
    {
        // Zender
        string msg = "Hello World";
        var alg = new HMACSHA384();
        alg.Key = Encoding.UTF8.GetBytes("Pa$$w0rd");
        byte[] hash = alg.ComputeHash(Encoding.UTF8.GetBytes(msg));
        Console.WriteLine(Convert.ToBase64String(hash));


        // Ed
        //msg += ".";


        // Ontvanger
        var alg2 = new HMACSHA384();
        alg2.Key = Encoding.UTF8.GetBytes("Pa$$w0rd");
        byte[] hash2 = alg2.ComputeHash(Encoding.UTF8.GetBytes(msg));
        Console.WriteLine(Convert.ToBase64String(hash2));
    }
    private static void TestHash()
    {
        // Zender
        string msg = "Hello World";
        SHA384 alg = SHA384.Create();
        byte[] hash = alg.ComputeHash(Encoding.UTF8.GetBytes(msg));
        Console.WriteLine(Convert.ToBase64String(hash));


        // Ed
        msg += ".";


        // Ontvanger
        SHA384 alg2 = SHA384.Create();
        byte[] hash2 = alg2.ComputeHash(Encoding.UTF8.GetBytes(msg));
        Console.WriteLine(Convert.ToBase64String(hash2));
    }
}
