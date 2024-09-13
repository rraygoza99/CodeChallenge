using DAL.Data.Interface;
using DAL.Data.Repository;
using DAL.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TechTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<TaskEntity>> GetAllTasks()
        {
            try
            {
                return new OkObjectResult(_taskRepository.GetAllTasks());
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult<TaskEntity> GetTaskById([FromRoute] int id)
        {
            try
            {
                return new OkObjectResult(_taskRepository.GetTaskById(id));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<TaskEntity> CreateTask([FromBody] TaskEntity task)
        {
            try
            {
                return new OkObjectResult(_taskRepository.CreateTask(task));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPut]
        public ActionResult<TaskEntity> UpdateTask([FromBody] TaskEntity entity)
        {
            try
            {
                return new OkObjectResult(_taskRepository.UpdateTask(entity));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpGet]
        [Route("complete")]
        public ActionResult<TaskEntity> ChangeTaskState([FromQuery] int id, [FromQuery] bool state)
        {
            try
            {
                return new OkObjectResult(_taskRepository.ChangeTaskState(id, state));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
