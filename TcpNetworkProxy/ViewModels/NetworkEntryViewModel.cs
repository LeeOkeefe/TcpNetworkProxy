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

    public int DataSize => Data.Split(" ").Length;

    private const int BytesPerPreview = 20;
    public string DataPreview
    {
        get
        {
            if (Data is null)
            {
                return null;
            }

            const int dataLength = BytesPerPreview * 2;
            const int delimitersLength = BytesPerPreview - 1;
            const int sum = dataLength + delimitersLength;

            return Data.Length <= sum
                ? Data
                : $"{Data[..sum]}...";
        }
    }
}