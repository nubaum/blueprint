using Blueprint.ViewModels.Pages;
using Blueprint.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.ViewModels;

public static class ViewModelDIConfigurator
{
    public static IServiceCollection ConfigureViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<DashboardViewModel>();
        services.AddSingleton<DataViewModel>();
        services.AddSingleton<CodeViewModel>();
        return services;
    }
}
