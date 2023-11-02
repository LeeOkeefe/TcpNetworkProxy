using System.Collections.ObjectModel;
using TcpNetworkProxy.Data;

namespace TcpNetworkProxy.ViewModels;

public sealed class NetworkEntriesViewModel : IDisposable
{
    public ObservableCollection<NetworkEntryViewModel> NetworkEntryViewModels { get; } = new();
    
    private readonly ProxyService _proxyService;

    public NetworkEntriesViewModel(ProxyService proxyService)
    {
        _proxyService = proxyService ?? throw new ArgumentNullException(nameof(proxyService));

        _proxyService.OnNetworkEntryAdded += UpdateNetworkEntryViewModels;
    }

    public void StartProxyServer(string host, int port, string destinationHost, int destinationPort)
        => _proxyService.StartProxyServer(host, port, destinationHost, destinationPort);
    
    private void UpdateNetworkEntryViewModels(NetworkEntry entry)
    {
        var model = ToViewModel(entry);
        NetworkEntryViewModels.Add(model);
    }

    private static NetworkEntryViewModel ToViewModel(NetworkEntry entry)
    {
        return new NetworkEntryViewModel
        {
            Timestamp = entry.Timestamp,
            Source = entry.Source,
            Destination = entry.Destination,
            Data = entry.Data
        };
    }

    public void Dispose()
    {
        _proxyService.OnNetworkEntryAdded -= UpdateNetworkEntryViewModels;
    }
}