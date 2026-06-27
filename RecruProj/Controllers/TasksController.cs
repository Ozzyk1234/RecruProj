using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecruProj.Dtos.TaskItemDtos;
using RecruProj.Models.Enums;
using RecruProj.Repository.TaskItemRepository;

namespace RecruProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskItemRepository _taskItemRepository;

        public TasksController(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<CreateUpdateTaskItemDTO>> UpdateTaskItem(int id, CreateUpdateTaskItemDTO taskItem) {
            var result = await _taskItemRepository.UpdateTaskItem(id, taskItem);

            if(result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<CreateUpdateTaskItemDTO>> UpdateTaskItemStatus(int id, Status status) {
            var result = await _taskItemRepository.UpdateTaskItemStatus(id, status);

            if(result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteTaskItem(int id) {
            var result = await _taskItemRepository.DeleteTaskItem(id);

            if(!result)
                return NotFound();

            return Ok(result);
        }
    }
}
