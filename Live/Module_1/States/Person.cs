

using System.Xml.Serialization;

namespace States;

[XmlRoot("person")]
public class Person
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    [XmlElement("name")]
    public string? Name { get; set; }
    [XmlElement("age")]
    public int Age { get; set; }

    public void Introduce()
    {
        Console.WriteLine($"Hi, I'm {Name} and I'm {Age} years old [{Id}]");
    }
}
