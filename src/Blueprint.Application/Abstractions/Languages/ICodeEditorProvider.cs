namespace Blueprint.Application.Abstractions.Languages;

public interface ICodeEditorProvider
{
    public object GetCodeEditor(object document);
}
