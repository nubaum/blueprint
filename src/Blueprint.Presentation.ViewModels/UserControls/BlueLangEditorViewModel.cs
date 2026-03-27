using ActiproSoftware.Text.Implementation;
using Blueprint.Abstractions.Application.Languages;
using Blueprint.Presentation.ViewModels.Core;
using Blueprint.Presentation.ViewModels.UserControls.Interfaces;

namespace Blueprint.Presentation.ViewModels.UserControls;

internal class BlueLangEditorViewModel : BindableObject, IBlueLangEditorViewModel
{
    private EditorDocument? _document;

    public BlueLangEditorViewModel(IUiCoreServices uiCoreServices, ILanguageProvider languageProvider)
        : base(uiCoreServices)
    {
        object lang = languageProvider.GetLanguage(SupportedLanguages.Blue);
        if (lang is SyntaxLanguage language)
        {
            Document = new EditorDocument { Language = language };
        }
    }

    public EditorDocument? Document
    {
        get => _document;
        set => SetField(ref _document, value);
    }
}
