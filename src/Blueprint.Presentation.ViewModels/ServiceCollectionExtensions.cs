using Blueprint.Presentation.ViewModels.Pages;
using Blueprint.Presentation.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Presentation.ViewModels;

public static class ServiceCollectionExtensions
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
