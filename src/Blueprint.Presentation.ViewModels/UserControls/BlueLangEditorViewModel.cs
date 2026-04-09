using Blueprint.Application.Core;
using Blueprint.Presentation.ViewModels.UserControls.Interfaces;

namespace Blueprint.Presentation.ViewModels.UserControls;

internal class BlueLangEditorViewModel : BindableObject, IBlueLangEditorViewModel
{
    public object? Document
    {
        get;
        set => SetField(ref field, value);
    }
}
