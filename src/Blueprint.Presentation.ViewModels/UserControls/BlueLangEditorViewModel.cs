using ActiproSoftware.Text.Implementation;
using Blueprint.Application.Core;
using Blueprint.Presentation.ViewModels.UserControls.Interfaces;

namespace Blueprint.Presentation.ViewModels.UserControls;

internal class BlueLangEditorViewModel : BindableObject, IBlueLangEditorViewModel
{
    public EditorDocument? Document
    {
        get;
        set => SetField(ref field, value);
    }
}
