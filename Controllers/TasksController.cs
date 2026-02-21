using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskApp.Api.DTOS;

[ApiController]
[Route("tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private Guid GetUserId()
    {
        return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    private bool IsAdmin()
    {
        return User.IsInRole("Admin");
    }

    // 🔎 DEBUG ENDPOINT
    [HttpGet("debug")]
    public IActionResult Debug()
    {
        var authHeader = Request.Headers["Authorization"].ToString();

        return Ok(new
        {
            AuthorizationHeader = authHeader,
            UserAuthenticated = User.Identity?.IsAuthenticated,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();
        var isAdmin = IsAdmin();

        var tasks = await _taskService.GetAllAsync(userId, isAdmin, page, pageSize);
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserId();
        var isAdmin = IsAdmin();

        var task = await _taskService.GetByIdAsync(id, userId, isAdmin);
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskDto dto)
    {
        var userId = GetUserId();
        var created = await _taskService.CreateAsync(dto, userId);
        return Ok(created);
    }

    [HttpPatch("{id:int}/complete")]
    public async Task<IActionResult> Complete(int id)
    {
        var userId = GetUserId();
        var isAdmin = IsAdmin();

        await _taskService.CompleteAsync(id, userId, isAdmin);
        return NoContent();
    }
}