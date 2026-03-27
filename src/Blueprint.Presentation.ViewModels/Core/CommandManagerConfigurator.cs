namespace Blueprint.Presentation.ViewModels.Core;

public static class CommandManagerConfigurator
{
    public static void SetCommandManager(ICommandManager commandManager)
    {
        CommandManagerHelper.CommandManager = commandManager;
    }
}
