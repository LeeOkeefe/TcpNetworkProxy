namespace TcpNetworkProxy.Extensions;

public static class ArrayExtensions
{
    public static string ToHexadecimalString(this byte[] bytes, int? bytesLength = null, string delimiter = " ")
    {
        var bytesToConvert = !bytesLength.HasValue || bytesLength > bytes.Length
            ? bytes 
            : bytes[..bytesLength.Value];
        
        return bytes is null ? null : string.Join(delimiter, bytesToConvert.Select(d => $"{d:X}"));
    }
}