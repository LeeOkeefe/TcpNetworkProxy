using TcpNetworkProxy.Validators;

namespace TcpNetworkProxy.ViewModels;

public sealed class NetworkEntryViewModel
{
    public TimeOnly Timestamp { get; set; }
    [IpAddress]
    public string Source { get; set; }
    [IpAddress]
    public string Destination { get; set; }
    [Hexadecimal]
    public string Data { get; set; }
}