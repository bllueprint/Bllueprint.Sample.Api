namespace Bllueprint.Application;

internal class AccessValidator : IAccessValidator
{
    public Task ValidateAccessAsync(Guid clientId)
    {
        if (clientId == Guid.Empty)
        {
            throw new InvalidOperationException("Can't access");
        }

        return Task.CompletedTask;
    }
}
