using Microsoft.AspNetCore.Mvc;
using TaskApp.Api.Services;
using TaskApp.Domain.Entities;

namespace TaskApp.Api.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskservice;

        public TasksController(ITaskService service)
        {
            _taskservice = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskservice.GetAllAsync();
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem task)
        {
            var created = await _taskservice.CreateAsync(task);
            return Ok(created);
        }
    }
}
