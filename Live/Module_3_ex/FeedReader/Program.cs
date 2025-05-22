using System.Xml.Serialization;
using System.Xml;

namespace FeedReader;

internal class Program
{
    static void Main(string[] args)
    {
        var stream = DoCallAsync().Result;
        var items = HandleStream(stream);
        foreach (var item in items)
        {
            Console.WriteLine($"** {item.Category}");
            Console.WriteLine($"- {item.Title}");
            Console.WriteLine(item.Description);
            Console.WriteLine();
        }

    }

    private static IEnumerable<Item?> HandleStream(Stream? stream)
    {
        if (stream == null) yield return null;

        var ser = new XmlSerializer(typeof(Item));
        var reader = XmlReader.Create(stream);
        //var items = new List<Item?>();
        while (reader.ReadToFollowing("item"))
        {
            var item = ser.Deserialize(reader.ReadSubtree()) as Item;
            //items.Add(item);
            yield return item;
        }
        //return items;
    }

    private static async Task<Stream?> DoCallAsync()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://www.nu.nl/");

        var response = await client.GetAsync("rss");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStreamAsync();
        }
        return null;
    }

}
