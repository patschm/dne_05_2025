namespace Attributen;

[Demo(MaxAge = 80, Age =45)]
public interface IMyClass
{
    string Name { get; set; }

    void DoeIets();
}