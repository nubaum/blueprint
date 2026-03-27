using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.Pages;
using Blueprint.Presentation.ViewModels.Pages.Interfaces;
using Blueprint.Presentation.ViewModels.UserControls;
using Blueprint.Presentation.ViewModels.UserControls.Interfaces;
using Blueprint.Presentation.ViewModels.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Blueprint.Presentation.ViewModels;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureViewModels(this IServiceCollection services)
    {
        services.ConfigureCoreServices();
        services.AddTransient<IMainWindowViewModel, MainWindowViewModel>();
        services.AddTransient<ISettingsViewModel, SettingsViewModel>();
        services.AddTransient<IDashboardViewModel, DashboardViewModel>();
        services.AddTransient<IDataViewModel, DataViewModel>();
        services.AddTransient<ICodeViewModel, CodeViewModel>();
        services.AddTransient<IBlueLangEditorViewModel, BlueLangEditorViewModel>();
        return services;
    }

    private static IServiceCollection ConfigureCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IUiCoreServices, UiCoreServices>();
        return services;
    }
}
