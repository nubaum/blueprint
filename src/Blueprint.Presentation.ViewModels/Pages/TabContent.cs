using Blueprint.Presentation.ViewModels.Core;

namespace Blueprint.Presentation.ViewModels.Pages;

public class TabContent : NotifyPropertyChangedBase
{
    private bool _isDirty;

    private bool _isPinned;

    public required string Caption { get; init; }

    public required object Content { get; init; }

    public object? Icon { get; init; }

    public bool IsDirty
    {
        get => _isDirty;
        set => SetField(ref _isDirty, value);
    }

    public bool IsPinned
    {
        get => _isPinned;
        set => SetField(ref _isPinned, value);
    }
}
