using System.Collections.ObjectModel;
using TcpNetworkProxy.Data;

namespace TcpNetworkProxy.ViewModels;

public sealed class NetworkEntriesViewModel : IDisposable
{
    public event Action<NetworkEntryViewModel> OnEntryAdded;
    public ObservableCollection<NetworkEntryViewModel> NetworkEntryViewModels { get; } = new();
    
    private readonly ProxyService _proxyService;

    public NetworkEntriesViewModel(ProxyService proxyService)
    {
        _proxyService = proxyService ?? throw new ArgumentNullException(nameof(proxyService));

        _proxyService.OnNetworkEntryAdded += UpdateNetworkEntryViewModels;
    }
    
    private void UpdateNetworkEntryViewModels(NetworkEntry entry)
    {
        var model = ToViewModel(entry);
        NetworkEntryViewModels.Add(model);
        
        OnEntryAdded?.Invoke(model);
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