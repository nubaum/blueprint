namespace Blueprint.Application;

public interface IAccessValidator
{
    Task ValidateAccessAsync(Guid clientId);
}
