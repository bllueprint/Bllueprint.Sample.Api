using Bllueprint.Application;
using Bllueprint.Core.Api;
using Microsoft.AspNetCore.Mvc;

namespace Bllueprint.Api;

[ApiController]
[Route("api/[controller]")]
public class TaskController : AppController
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync() => await SendAsync(new GetAllTasksQuery());

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromQuery] string title) => await SendAsync(new CreateTaskCommand(title));

    [HttpPut("{id}/start")]
    public async Task<IActionResult> StartAsync(Guid id) => await SendAsync(new StartTaskCommand(id));

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteAsync(Guid id) => await SendAsync(new CompleteTaskCommand(id));

    [HttpPut("{id}/reopen")]
    public async Task<IActionResult> ReopenAsync(Guid id) => await SendAsync(new ReopenTaskCommand(id));
}
