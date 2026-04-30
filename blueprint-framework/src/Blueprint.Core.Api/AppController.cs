using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blueprint.Core.Api;

[ApiController]
public abstract class AppController : ControllerBase
{
    protected IMediator Mediator
    {
        get =>
        field ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }

    protected Task<IActionResult> SendAsync<T>(IRequest<ICommandResult<T>> command)
        => Mediator
            .Send(command)
            .ToActionResultAsync(this);
}
