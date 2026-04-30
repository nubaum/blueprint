using Microsoft.AspNetCore.Mvc;

namespace Blueprint.Core.Api;

internal static class CommandResultExtensions
{
    public static async Task<IActionResult> ToActionResultAsync<T>(
        this Task<ICommandResult<T>> resultTask,
        ControllerBase controller)
    {
        ICommandResult<T> result = await resultTask;

        if (result.NotFound)
        {
            return controller.NotFound();
        }

        if (!result.HasErrors)
        {
            return controller.Ok(result.Entity);
        }

        var errors = new
        {
            errors = result.Errors.Select(e => new
            {
                transition = e.TransitionName,
                message = e.Message,
                kind = e.Kind.ToString()
            })
        };

        if (result.Errors.Any(o => o.Kind == Domain.NotificationKind.InternalError))
        {
            return controller.StatusCode(StatusCodes.Status500InternalServerError, errors);
        }

        return controller.BadRequest(errors);
    }
}
