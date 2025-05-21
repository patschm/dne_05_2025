
using System.Xml;
using System.Xml.Serialization;

namespace States;

internal class Program
{
    static void Main(string[] args)
    {
        Person p1 = new Person { Id = 1, Name="Jan", Age=45 };
        Person p2 = new Person { Id = 2, Name = "Marieke", Age = 34 };

        //SaveToDisk(p1);
        //UsingSerializer(p1, p2);
        ReadUsingSerializer(out Person[] people);

        //p1.Introduce();
        //p2.Introduce();
        people[0].Introduce();
        people[1].Introduce();
    }

    private static void ReadUsingSerializer(out Person[]? people)
    {
        FileStream stream = File.OpenRead(@"D:\TestData\people2.xml");
        XmlSerializer ser = new XmlSerializer(typeof(Person[]));
        people = ser.Deserialize(stream) as Person[];

    }

    private static void UsingSerializer(params Person[] values)
    {
        FileStream stream = File.Create(@"D:\TestData\people2.xml");
        XmlSerializer ser = new XmlSerializer(typeof(Person[]));
        ser.Serialize(stream, values);

    }

    private static void SaveToDisk(Person p)
    {
        FileStream stream = File.Create(@"D:\TestData\people.xml");
        XmlWriter writer = XmlWriter.Create(stream);
        writer.WriteStartElement("people");
        writer.WriteStartElement("person");
        writer.WriteAttributeString("id", p.Id.ToString());
        writer.WriteStartElement("name");
        writer.WriteString(p.Name); 
        writer.WriteEndElement();
        writer.WriteStartElement("age");
        writer.WriteString(p.Age.ToString());   
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.Flush();
        writer.Close();
    }
}
