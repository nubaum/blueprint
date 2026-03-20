using Blueprint.ViewModels.Core;

namespace Blueprint.ViewModels.UserControls;

public class DocumentTabViewModel : NotifyPropertyChangedBase, ITabViewModel
{
    private string _caption = "Untitled";
    private object? _icon;
    private bool _isPinned;
    private bool _isDirty;

    public string Caption
    {
        get => _caption;
        set => SetField(ref _caption, value);
    }

    public object? Icon
    {
        get => _icon;
        set => SetField(ref _icon, value);
    }

    public bool IsPinned
    {
        get => _isPinned;
        set => SetField(ref _isPinned, value);
    }

    public bool IsDirty
    {
        get => _isDirty;
        set => SetField(ref _isDirty, value);
    }

    public string FilePath { get; set; } = string.Empty;

    public object Content { get; set; } = string.Empty;
}
