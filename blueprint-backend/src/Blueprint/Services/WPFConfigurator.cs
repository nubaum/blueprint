using Blueprint.Presentation.Adapters;
using Blueprint.Presentation.Adapters.Abstractions;

namespace Blueprint.Services;

internal static class WpfConfigurator
{
    public static void ConfigureWpfDependencies()
    {
        AddWindowsSynchronizer();
        AddWindowsCommandManager(new WindowsCommandManager());
    }

    private static void AddWindowsSynchronizer()
    {
        UIDispatcher.UIScynchronizer = new UISynchronizer();
    }

    private static void AddWindowsCommandManager(ICommandManager commandManager)
    {
        CommandManagerHelper.CommandManager = commandManager;
    }
}
