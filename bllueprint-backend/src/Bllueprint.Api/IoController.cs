using Bllueprint.Application.Abstractions.Infrastructure.Models;
using Bllueprint.Application.Messages.FolderManagement;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bllueprint.Api;

[ApiController]
[Route("api/[controller]")]
public class IoController(IMediator mediator) : ControllerBase
{
    [HttpGet("folder-structure")]
    public async Task<IActionResult> GetAllAsync([FromQuery] string path)
    {
        FileSystemItem result = await mediator.Send(new GetFolderStructureRequest { RootPath = path });
        return Ok(result);
    }
}
