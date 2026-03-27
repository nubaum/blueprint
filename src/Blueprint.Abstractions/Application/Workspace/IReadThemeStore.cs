namespace Blueprint.Abstractions.Application.Workspace;

public interface IReadThemeStore
{
    BlueprintTheme CurrentTheme { get; }
}
