using FluentValidation.Results;

namespace Blueprint.Domain.Languages.Core;

public abstract class Ast<T> : Ast
    where T : class, new()
{
    public T? Data { get; private set; }

    public void SetData(T data, ValidationResult validationResults)
    {
        Data = data;
        ClearSemanticErrors();
        AddErrors(validationResults);
    }

    private void AddErrors(ValidationResult? validationResult)
    {
        if (validationResult is not null && validationResult.Errors.Count > 0)
        {
            AddErrors(validationResult.Errors.Where(o => o.CustomState is ParseErrorInfo).Select(o => (ParseErrorInfo)o.CustomState));
        }
    }
}
