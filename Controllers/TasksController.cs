using Microsoft.AspNetCore.Mvc;
using TaskApp.Api.Services;

namespace TaskApp.Api.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskservice;
        public TasksController(ITaskService Service)
        {
            _taskservice = Service;

        }
        [HttpGet]


        public IActionResult GetAll()
        {
            return Ok(_taskservice.GetAll());

        }
    }
}
