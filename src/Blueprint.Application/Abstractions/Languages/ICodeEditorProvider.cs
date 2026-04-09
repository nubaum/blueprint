namespace Blueprint.Abstractions.Application.Languages;

public interface ICodeEditorProvider
{
    public object GetCodeEditor(object document);
}
