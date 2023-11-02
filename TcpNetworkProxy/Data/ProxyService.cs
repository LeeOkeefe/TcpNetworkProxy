using System.Net;
using System.Net.Sockets;

namespace TcpNetworkProxy.Data;

public sealed class ProxyService
{
    public event Action<NetworkEntry> OnNetworkEntryAdded;

    private readonly CancellationTokenSource _cts = new();

    private string _proxyHost;
    private string _destinationHost;
    
    public async void StartProxyServer(string host, int port, string destinationHost, int destinationPort)
    {
        _proxyHost = host ?? throw new ArgumentNullException(nameof(host));
        _destinationHost = destinationHost ?? throw new ArgumentNullException(nameof(destinationHost));
        
        var listener = new TcpListener(IPAddress.Parse(host), port);
        listener.Start();

        while (!_cts.IsCancellationRequested)
        {
            var incomingClient = await listener.AcceptTcpClientAsync();
            var outgoingClient = new TcpClient(destinationHost, destinationPort);
            
            _ = Task.Run(() => HandleClientAsync(incomingClient, outgoingClient));
        }
        
        listener.Stop();
    }

    private async Task HandleClientAsync(TcpClient incomingClient, TcpClient outgoingClient)
    {
        await using var incomingStream = incomingClient.GetStream();
        await using var outgoingStream = outgoingClient.GetStream();
        
        var incomingToOutgoing = ForwardDataAsync(incomingStream, outgoingStream, _proxyHost, _destinationHost);
        var outgoingToIncoming = ForwardDataAsync(outgoingStream, incomingStream, _destinationHost, _proxyHost);
        
        await Task.WhenAll(incomingToOutgoing, outgoingToIncoming);
    }
    
    private async Task ForwardDataAsync(Stream fromStream, Stream toStream, string source, string destination)
    {
        var buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = await fromStream.ReadAsync(buffer)) > 0)
        {
            await toStream.WriteAsync(buffer.AsMemory(0, bytesRead));
            await toStream.FlushAsync();

            var data = BitConverter.ToString(buffer, 0, bytesRead);
            var entry = new NetworkEntry(TimeOnly.FromDateTime(DateTime.Now), source, destination, data);
            
            OnNetworkEntryAdded?.Invoke(entry);
        }
    }
}