using System.Net;
using System.Net.Sockets;

namespace TcpNetworkProxy.Data;

public sealed class ProxyService
{
    public event Action<NetworkEntry> OnNetworkDataSent;

    private readonly CancellationTokenSource _cts = new();

    private TcpClient _incomingClient;
    private TcpClient _outgoingClient;
    
    private Connection _proxy;
    private Connection _destination;
    
    public async void StartProxyServer(Connection proxy, Connection destination)
    {
        _proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
        _destination = destination ?? throw new ArgumentNullException(nameof(destination));
        
        var listener = new TcpListener(IPAddress.Parse(proxy.Host), proxy.Port);
        listener.Start();

        while (!_cts.IsCancellationRequested)
        {
            _incomingClient = await listener.AcceptTcpClientAsync();
            _outgoingClient = new TcpClient(destination.Host, destination.Port);
            
            _ = Task.Run(() => HandleClientAsync(_incomingClient, _outgoingClient));
        }
        
        listener.Stop();
    }

    public async Task SendCustomNetworkEntry(NetworkEntry networkEntry)
    {
        var targetStream = networkEntry.Destination == _destination.Host
            ? _outgoingClient.GetStream()
            : _incomingClient.GetStream();
        
        await targetStream.WriteAsync(networkEntry.Data);
        await targetStream.FlushAsync();
            
        OnNetworkDataSent?.Invoke(networkEntry);
    }
    
    private async Task HandleClientAsync(TcpClient incomingClient, TcpClient outgoingClient)
    {
        await using var incomingStream = incomingClient.GetStream();
        await using var outgoingStream = outgoingClient.GetStream();
        
        var incomingToOutgoing = ForwardDataAsync(incomingStream, outgoingStream, _proxy, _destination);
        var outgoingToIncoming = ForwardDataAsync(outgoingStream, incomingStream, _destination, _proxy);
        
        await Task.WhenAll(incomingToOutgoing, outgoingToIncoming);
    }

    private async Task ForwardDataAsync(Stream fromStream, Stream toStream, Connection source, Connection destination)
    {
        var buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = await fromStream.ReadAsync(buffer)) > 0)
        {
            await toStream.WriteAsync(buffer.AsMemory(0, bytesRead));
            await toStream.FlushAsync();

            var data = new byte[bytesRead];
            Array.Copy(buffer, data, bytesRead);

            var entry = new NetworkEntry(TimeOnly.FromDateTime(DateTime.Now), source.Host, destination.Host, data);

            OnNetworkDataSent?.Invoke(entry);
        }
    }
}