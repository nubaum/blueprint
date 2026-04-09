using ActiproSoftware.Text.Implementation;
using Blueprint.Application.Abstractions.Languages;
using Blueprint.Presentation.ViewModels.UserControls.Interfaces;
using Blueprint.Views.UserControls;

namespace Blueprint.Services;

public class CodeEditorProvider : ICodeEditorProvider
{
    public object GetCodeEditor(object document)
    {
        var result = new BlueLangEditor();
        if (document is EditorDocument doc && result.DataContext is IBlueLangEditorViewModel viewModel)
        {
            viewModel.Document = doc;
        }

        return result;
    }
}
