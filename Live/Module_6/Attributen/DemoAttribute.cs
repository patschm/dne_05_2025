
namespace Attributen;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
internal class DemoAttribute : Attribute
{
    public int Age { get; set; }
    public int MaxAge { get; set; }  

    public bool IsValidAge(int age)
    {
        return age < MaxAge;
    }
}
