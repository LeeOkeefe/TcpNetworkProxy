using TcpNetworkProxy.Validators;

namespace TcpNetworkProxy.Data;

public sealed class Connection
{
    public string Host { get; }
    public int Port { get; }

    private readonly IpAddressAttribute _ipAddress = new();
    
    public Connection(string host, int port)
    {
        if (!_ipAddress.IsValid(host))
        {
            throw new ArgumentException("Invalid IP address", nameof(host));
        }

        if (port is < ushort.MinValue or > ushort.MaxValue)
        {
            throw new ArgumentException($"Port must be between {short.MinValue} and {short.MaxValue}", nameof(port));
        }
        
        Host = host;
        Port = port;
    }
}