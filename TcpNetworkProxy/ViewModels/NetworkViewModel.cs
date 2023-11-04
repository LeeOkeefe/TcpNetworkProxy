using System.Collections.Immutable;
using TcpNetworkProxy.Data;
using TcpNetworkProxy.Extensions;

namespace TcpNetworkProxy.ViewModels;

public sealed class NetworkViewModel : IDisposable
{
    public IReadOnlyList<NetworkEntryViewModel> GetNetworkEntriesSnapshot() => _displayEntries;
    public event Action OnUpdateRequested;

    private ImmutableList<NetworkEntryViewModel> _displayEntries = ImmutableList<NetworkEntryViewModel>.Empty;
    private readonly List<NetworkEntry> _pendingEntries = new();
    private readonly ProxyService _proxyService;
    private readonly System.Timers.Timer _updateTimer;
    private const int MaxNetworkEntries = 100;
    
    public NetworkViewModel(ProxyService proxyService)
    {
        _proxyService = proxyService;
        _proxyService.OnNetworkDataSent += OnNetworkDataSent;
        
        _updateTimer = new System.Timers.Timer(200);
        _updateTimer.Elapsed += (_, _) =>
        {
            ProcessPendingEntries();
            PruneNetworkEntries();
            OnUpdateRequested?.Invoke();
        };
        _updateTimer.Start();
    }

    public void StartProxyServer(string host, int port, string destinationHost, int destinationPort)
    {
        var proxy = new Connection(host, port);
        var destination = new Connection(destinationHost, destinationPort);
        
        _proxyService.StartProxyServer(proxy, destination);
    }
    
    private void ProcessPendingEntries()
    {
        List<NetworkEntry> entriesToProcess;
        lock (_pendingEntries)
        {
            entriesToProcess = new List<NetworkEntry>(_pendingEntries);
            _pendingEntries.Clear();
        }

        var viewModels = entriesToProcess.Select(e => e.ToViewModel());
        _displayEntries = _displayEntries.AddRange(viewModels);
    }
    
    private void PruneNetworkEntries()
    {
        if (_displayEntries.Count < MaxNetworkEntries)
        {
            return;
        }
        
        _displayEntries = _displayEntries.RemoveRange(0, _displayEntries.Count - MaxNetworkEntries);
    }
    
    private void OnNetworkDataSent(NetworkEntry networkEntry)
    {
        lock (_pendingEntries)
        {
            _pendingEntries.Add(networkEntry);
        }
    }

    public void Dispose()
    {
        _proxyService.OnNetworkDataSent -= OnNetworkDataSent;
        
        if (_updateTimer == null)
        {
            return;
        }
        
        _updateTimer.Stop();
        _updateTimer.Dispose();
    }
}