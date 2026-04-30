namespace Blueprint.Core.Api.Example;

public interface IAccessValidator
{
    Task ValidateAccessAsync(Guid clientId);
}
