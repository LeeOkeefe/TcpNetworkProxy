using TcpNetworkProxy.Data;
using TcpNetworkProxy.ViewModels;

namespace TcpNetworkProxy.Extensions;

public static class NetworkEntryExtensions
{
    public static NetworkEntryViewModel ToViewModel(this NetworkEntry entry)
    {
        return new NetworkEntryViewModel
        {
            Timestamp = entry.Timestamp,
            Source = entry.Source,
            Destination = entry.Destination,
            Data = entry.Data
        };
    }
}