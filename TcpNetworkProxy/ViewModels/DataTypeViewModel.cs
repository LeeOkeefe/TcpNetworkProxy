namespace TcpNetworkProxy.ViewModels;

public class DataTypeViewModel
{
    public Type Type { get; set; }
    public int SizeInBytes { get; set; }
    
    public override bool Equals(object o) {
        var other = o as DataTypeViewModel;
        return other?.Type == Type;
    }
    
    public override int GetHashCode() => Type.GetHashCode();
}