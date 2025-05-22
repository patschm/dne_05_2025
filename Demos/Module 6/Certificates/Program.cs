using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;

namespace Certificates;


// $params = @{
//    Type = 'Custom'
//    Subject = 'E=patti.fuller@contoso.com,CN=Patti Fuller'
//    TextExtension = @(
//        '2.5.29.37={text}1.3.6.1.5.5.7.3.4',
//        '2.5.29.17={text}email=patti.fuller@contoso.com&email=pattifuller@contoso.com' )
//    KeyAlgorithm = 'RSA'
//    KeyLength = 2048
//    SmimeCapabilities = $true
//    CertStoreLocation = 'Cert:\CurrentUser\My'
//}
//New - SelfSignedCertificate @params

internal class Program
{
    // Run the following command first
    // makecert.exe -sr CurrentUser -ss MY -a sha1 -n CN=ClientCert -sky exchange -pe
    static void Main(string[] args)
    {
        X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadOnly);
        X509Certificate2Collection certificates = store.Certificates;
        foreach (X509Certificate2 cert in certificates)
        {
            Console.WriteLine(cert.SubjectName.Name);
        }
        var certificate = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, "E=patti.fuller@contoso.com, CN=Patti Fuller", false).FirstOrDefault(); ;
        Console.WriteLine($"Found Certificate {certificate?.SubjectName.Name}");

        // Encrypt
        RSA alg = certificate?.GetRSAPublicKey()!;
        byte[] cipher = alg.Encrypt(Encoding.UTF8.GetBytes("Hello World"), RSAEncryptionPadding.OaepSHA1);
        Console.WriteLine(Convert.ToBase64String(cipher));

        // Decrypt
        RSA alg2 = certificate?.GetRSAPrivateKey()!;
        byte[] data = alg2.Decrypt(cipher, RSAEncryptionPadding.OaepSHA1);
        Console.WriteLine(Encoding.UTF8.GetString(data));
    }
}