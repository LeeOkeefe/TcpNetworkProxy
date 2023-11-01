using System.ComponentModel.DataAnnotations;
using TcpNetworkProxy.Validators;

namespace TcpNetworkProxy.ViewModels;

public sealed class EstablishConnectionViewModel
{
    [Display(Name = "proxy host")]
    [Required]
    [IpAddress(ErrorMessage = "The field {0} must be a valid IP address")]
    public string ProxyHost { get; set; }
    
    [Display(Name = "proxy port")]
    [Range(ushort.MinValue, ushort.MaxValue)]
    public int ProxyPort { get; set; }
    
    [Display(Name = "destination host")]
    [Required]
    [IpAddress(ErrorMessage = "The field {0} must be a valid IP address")]
    public string DestinationHost { get; set; }
    
    [Display(Name = "destination port")]
    [Range(ushort.MinValue, ushort.MaxValue)]
    public int DestinationPort { get; set; }
}