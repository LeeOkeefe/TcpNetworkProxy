namespace TcpNetworkProxy.Data;

public sealed class NetworkEntry
{
    public string Source { get; }
    public string Destination { get; }
    public string Data { get; }
    
    public NetworkEntry(string source, string destination, string data)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source));
        Destination = destination ?? throw new ArgumentNullException(nameof(destination));
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }
}