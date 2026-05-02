namespace Bllueprint.Application;

public interface IAccessValidator
{
    Task ValidateAccessAsync(Guid clientId);
}
