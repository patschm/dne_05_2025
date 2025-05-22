
using System.Security.Cryptography;
using System.Text;

namespace Vertrouwelijk;

internal class Program
{
    static void Main(string[] args)
    {
        //Asymmetrisch();
        Symmetrisch();
    }

    private static void Symmetrisch()
    {
        // Sender
        string msg = "Hello World";
        Aes alg = Aes.Create();
        alg.Mode = CipherMode.CBC;
        byte[] key = alg.Key;
        byte[] iv = alg.IV;

        byte[] cipher;
        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream crypt = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new StreamWriter(crypt))
                {
                    sw.WriteLine(msg);
                }
            }
            cipher = ms.ToArray();
        }
        Console.WriteLine(Encoding.Default.GetString(cipher));



        // Ontvanger
        Aes alg2 = Aes.Create();
        alg2.Mode = CipherMode.CBC;
        alg2.Key = key;
        alg2.IV = iv;
        using (MemoryStream ms = new MemoryStream(cipher))
        using (CryptoStream crypt = new CryptoStream(ms, alg2.CreateDecryptor(), CryptoStreamMode.Read))
        using (StreamReader sw = new StreamReader(crypt))
        {
            string data = sw.ReadToEnd();
            Console.WriteLine(data);
        }

    }

    private static void Asymmetrisch()
    {
        // Ontvanger genereert pub/priv key
        RSA alg0 = RSA.Create();
        string privateKey = alg0.ToXmlString(true);
        string publicKey = alg0.ToXmlString(false);


        // Sender
        string msg = "Hello World";
        RSA rsa1 = RSA.Create();
        rsa1.FromXmlString(publicKey);
        byte[] crypt = rsa1.Encrypt(Encoding.UTF8.GetBytes(msg), RSAEncryptionPadding.Pkcs1);
        Console.WriteLine(Encoding.UTF8.GetString(crypt));


        // Ontvanger
        RSA rsa2 = RSA.Create();
        rsa2.FromXmlString(privateKey);
        byte[] cdata = rsa2.Decrypt(crypt, RSAEncryptionPadding.Pkcs1);
        Console.WriteLine(Encoding.UTF8.GetString(cdata));
    }
}
