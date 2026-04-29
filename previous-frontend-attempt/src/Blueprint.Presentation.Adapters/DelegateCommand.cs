namespace Blueprint.Presentation.Adapters;

public class DelegateCommand : DelegateCommand<object>
{
    public DelegateCommand(Action execute)
        : base(_ => execute())
    {
    }

    public DelegateCommand(Action execute, Func<bool> canExecute)
        : base(_ => execute(), _ => canExecute())
    {
    }
}
