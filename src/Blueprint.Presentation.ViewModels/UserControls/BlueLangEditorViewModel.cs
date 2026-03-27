using ActiproSoftware.Text.Implementation;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.UserControls.Interfaces;

namespace Blueprint.Presentation.ViewModels.UserControls;

internal class BlueLangEditorViewModel : BindableObject, IBlueLangEditorViewModel
{
    private EditorDocument? _document;

    public EditorDocument? Document
    {
        get => _document;
        set => SetField(ref _document, value);
    }
}
