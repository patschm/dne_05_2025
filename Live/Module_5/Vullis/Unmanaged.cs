
namespace Vullis;

internal class Unmanaged : IDisposable
{
    private static bool _isOpen = false;
    private FileStream _stream;

    public void Open()
    {
        if (_isOpen)
        {
            Console.WriteLine("Kan niet. Is al in gebruik");
            return;
        }

        Console.WriteLine("Doe je ding");
        _stream = File.Create("D:\\dummy.txt");
        _isOpen = true;
    }

    public void Close() 
    {
        Console.WriteLine("Closing....");
        _isOpen = false;
    }

    protected void RuimOp(bool fromDispose)
    {
        Close();
        if (fromDispose)
        {
            _stream?.Dispose();
        }
    }
    public void Dispose()
    {
        RuimOp(true);
        GC.SuppressFinalize(this);
    }

    ~Unmanaged()
    {
        RuimOp(false);
    }
}
