using Blueprint.Application.Abstractions.Workspace;

namespace Blueprint.Application.InternalAbstractions;

internal interface IWriteThemeStore : IReadThemeStore
{
    void SetTheme(BlueprintTheme parameter);
}
