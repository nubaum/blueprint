using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Blueprint.Presentation.ViewModels.Core;

public abstract class BindableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = default!)
    {
        UIDispatcher.RunOnUiThread(() => PropertyChanged?.Invoke(this, new(name)));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string name = null!)
    {
        if (Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(name);
        return true;
    }
}
