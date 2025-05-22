namespace SomeLibrary;

public class Person
{
    private int _age;

    public int Age
    {
        get { return _age; }
        set 
        { 
            if (value >= 0 && value < 125) 
                _age = value; 
        }
    }

    public string? Firstname { get; set; }
    public string? LastName { get; set; }

    public void Introduce()
    {
        Console.WriteLine($"Hello. I'm {Firstname} {LastName} and I'm {Age} years old");
    }
}
