using MudBlazor.Services;
using TcpNetworkProxy.ViewModels;

namespace TcpNetworkProxy.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddTransient<ConnectionViewModel>();
        services.AddTransient<NetworkEntryViewModel>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        services.AddMudServices();
        services.AddMauiBlazorWebView();

        return services;
    }
}