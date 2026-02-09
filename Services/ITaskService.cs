using TaskApp.Api.DTOS;

namespace TaskApp.Api.Services
{
    public interface ITaskService
    {
       IEnumerable<TaskDto> GetAll();
    }

}

