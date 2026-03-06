using System.ComponentModel;

namespace Blueprint.ViewModels.Primitives;

public interface ITabViewModel : INotifyPropertyChanged
{
    string Caption { get; }
    object? Icon { get; }
    bool IsPinned { get; set; }
    bool IsDirty { get; }
}