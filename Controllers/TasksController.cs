using Microsoft.AspNetCore.Mvc;

namespace TaskApp.Api.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new List<string>());
        }
    }
}
