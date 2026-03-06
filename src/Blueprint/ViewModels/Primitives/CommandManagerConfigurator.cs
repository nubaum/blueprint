namespace Blueprint.ViewModels.Primitives;

public static class CommandManagerConfigurator
{
    public static void SetCommandManager(ICommandManager commandManager)
    {
        CommandManagerHelper.CommandManager = commandManager;
    }
}
