using Microsoft.AspNetCore.Mvc;
using TaskApp.Api.DTOS;
using TaskApp.Api.Services;

namespace TaskApp.Api.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var tasks = await _taskService.GetAllAsync(page, pageSize);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            var created = await _taskService.CreateAsync(dto);
            return Ok(created);
        }
    }
}
