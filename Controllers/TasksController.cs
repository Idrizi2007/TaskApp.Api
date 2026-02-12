using Microsoft.AspNetCore.Mvc;
using TaskApp.Api.DTOS;
using TaskApp.Api.DTOS;
using TaskApp.Api.Services;

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
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            var created = await _taskservice.CreateAsync(dto);
            return Ok(created);
        }
    }
}
