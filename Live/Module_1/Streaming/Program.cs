
using System.IO.Compression;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Streaming;
     
class Program
{
    static void Main()
    {
        //StreamWritesPrehistory();
        //StreamReadsPrehistory();

        //StreamWritesModern();
       // StreamReadsModern();


        //StreamWritesZipped();
        StreamReadsZipped();
    }

    private static void StreamWritesZipped()
    {
        FileStream stream = File.Create(@"D:\TestData\file1.zip");
        GZipStream zip = new GZipStream(stream, CompressionMode.Compress);
        StreamWriter writer = new StreamWriter(zip);
        for (int i = 0; i < 1000; i++)
        {
            writer.WriteLine($"Hello World {i}");
        }
        writer.Flush();
        writer.Close();
    }

    private static void StreamReadsZipped()
    {
        FileStream stream = File.OpenRead(@"D:\TestData\file1.zip");
        GZipStream zip = new GZipStream (stream, CompressionMode.Decompress);
        StreamReader reader = new StreamReader(zip);
        string? line;// = null;
        while ((line = reader.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
    }

    private static void StreamReadsModern()
    {
        FileStream stream = File.OpenRead(@"D:\TestData\file1.txt");
        StreamReader reader = new StreamReader(stream);
        string? line;// = null;
        while((line = reader.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }

    }

    private static void StreamWritesModern()
    {
        FileStream stream = File.Create(@"D:\TestData\file1.txt");
        StreamWriter writer = new StreamWriter(stream);
        for (int i = 0; i < 1000; i++)
        {
            writer.WriteLine($"Hello World {i}");
        }
        writer.Flush();
        writer.Close();
    }

    private static void StreamReadsPrehistory()
    {
        FileStream stream = File.OpenRead(@"D:\TestData\textfile.txt");
        byte[] buffer = new byte[20];
        int nrRead = 0;
        while ((nrRead = stream.Read(buffer)) > 0)
        {
            string txt = Encoding.UTF8.GetString(buffer);
            Console.Write(txt);
            Array.Clear(buffer, 0, nrRead);
        }
        
    }

    private static void StreamWritesPrehistory()
    {
        DirectoryInfo dinf = new DirectoryInfo(@"D:\TestData");
        if (!dinf.Exists)
        {
            dinf.Create();
        }

        //if (!Directory.Exists(@"D:\TestData"))
        //{
        //    Directory.CreateDirectory(@"D:\TestData");
        //}

        string txt = "Hello World ";
        FileInfo fInfo = new FileInfo($@"{dinf.FullName}\textfile.txt");
        if (!fInfo.Exists)
        {
           FileStream stream =  fInfo.Create();
            for (int i = 0; i < 1000; i++)
            {
                byte[] data = Encoding.UTF8.GetBytes(txt + i + "\r\n");
                stream.Write(data, 0, data.Length);
             }

            stream.Flush();
            stream.Close();
        }
        else
        {
            fInfo.Delete();
        }

        //File.Exists(@"D:\TestData\textfile.txt");


    }
}
