using System.Windows.Input;
using Blueprint.Abstractions.Application.Workspace;

namespace Blueprint.Application.InternalAbstractions;

internal interface IWriteThemeStore : IReadThemeStore
{
    ICommand ChangeThemeCommand { get; }
}
