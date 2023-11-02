using System.ComponentModel.DataAnnotations;

namespace TcpNetworkProxy.Validators;

public sealed class IpAddressAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var host = value as string;
        if (string.IsNullOrWhiteSpace(host) || host.Count(c => c == '.') != 3)
        {
            return false;
        }
        
        var octaves = host.Split(".");

        return octaves.Length == 4 && octaves.All(octave => byte.TryParse(octave, out _));
    }
}