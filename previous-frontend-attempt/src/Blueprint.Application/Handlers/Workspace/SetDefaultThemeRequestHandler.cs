using Blueprint.Application.Abstractions.Workspace;
using Blueprint.Application.InternalAbstractions;
using Blueprint.Application.Requests;
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
