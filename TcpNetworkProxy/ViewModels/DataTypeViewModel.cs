namespace TcpNetworkProxy.ViewModels;

public class DataTypeViewModel
{
    public string TypeName { get; set; }
    public int SizeInBytes { get; set; }
    
    public override bool Equals(object o) {
        var other = o as DataTypeViewModel;
        return other?.TypeName == TypeName;
    }
    
    public override int GetHashCode() => TypeName.GetHashCode();
}