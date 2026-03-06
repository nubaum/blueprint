using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Blueprint.ViewModels.Primitives;

namespace Blueprint.ViewModels.UserControls;

public class CodeViewModel
{
    public ObservableCollection<DocumentTabViewModel> Tabs { get; } = new();

    private DocumentTabViewModel? _selectedTab;
    public DocumentTabViewModel? SelectedTab
    {
        get => _selectedTab;
        set { _selectedTab = value; OnPropertyChanged(); }
    }

    public CodeViewModel()
    {
        Tabs.Add(new DocumentTabViewModel { Caption = "Welcome", IsPinned = true, Content = "Content 1" });
        Tabs.Add(new DocumentTabViewModel { Caption = "Document 1", IsDirty = true, Content = "Content 2" });
        Tabs.Add(new DocumentTabViewModel { Caption = "Document 2", Content = "Content 3"});
        SelectedTab = Tabs[0];
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class DocumentTabViewModel : ITabViewModel
{
    private string _caption = "Untitled";
    public string Caption
    {
        get => _caption;
        set { _caption = value; OnPropertyChanged(); }
    }

    private object? _icon;
    public object? Icon
    {
        get => _icon;
        set { _icon = value; OnPropertyChanged(); }
    }

    private bool _isPinned;
    public bool IsPinned
    {
        get => _isPinned;
        set { _isPinned = value; OnPropertyChanged(); }
    }

    private bool _isDirty;
    public bool IsDirty
    {
        get => _isDirty;
        set { _isDirty = value; OnPropertyChanged(); }
    }


    public string FilePath { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    // ... Commands, sub-ViewModels, whatever you need ...

    // -------------------------------------------------------------------------
    // INPC boilerplate
    // -------------------------------------------------------------------------

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}