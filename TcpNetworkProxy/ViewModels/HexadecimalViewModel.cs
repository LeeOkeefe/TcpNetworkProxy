namespace TcpNetworkProxy.ViewModels;

public class HexadecimalViewModel
{
    public byte Byte { get; set; }
    public bool IsSelected { get; set; }
    public int Index { get; set; }

    public override string ToString()
    {
        return $"{Byte:X2}";
    }
}