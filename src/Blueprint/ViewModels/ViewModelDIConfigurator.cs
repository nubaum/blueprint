using Blueprint.ViewModels.Pages;
using Blueprint.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.ViewModels;

public static class ViewModelDIConfigurator
{
    public static IServiceCollection ConfigureViewModels(this IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<DataViewModel>();
        services.AddTransient<CodeViewModel>();
        return services;
    }
}
