namespace TcpNetworkProxy.Data;

public sealed class ProxyService : IDisposable
{
    public event Action<NetworkEntry> OnNetworkEntryAdded;

    public void AddEntry(NetworkEntry entry)
    {
        OnNetworkEntryAdded?.Invoke(entry);
    }
    
    public void Dispose()
    {
        // TODO release managed resources here
    }
}