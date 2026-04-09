using Blueprint.Abstractions.Application.Workspace;
using Blueprint.Abstractions.Messages.Workspace;
using Blueprint.Application.InternalAbstractions;
using MediatR;

namespace Blueprint.Application.Handlers.Workspace;

internal class SetDefaultThemeRequestHandler(IWriteThemeStore writeThemeStore) : IRequestHandler<SetDefaultThemeRequest>
{
    public async Task Handle(SetDefaultThemeRequest request, CancellationToken cancellationToken)
    {
        writeThemeStore.SetTheme(BlueprintTheme.Dark);
        await Task.CompletedTask;
    }
}
