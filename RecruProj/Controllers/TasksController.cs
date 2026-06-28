using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecruProj.Dtos.TaskItemDtos;
using RecruProj.Models.Enums;
using RecruProj.Models.TaskItemName;
using RecruProj.Repository.TaskItemRepository;

namespace RecruProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IValidator<TaskItem> _validator;

        public TasksController(ITaskItemRepository taskItemRepository, IValidator<TaskItem> validator)
        {
            _taskItemRepository = taskItemRepository;
            _validator = validator;
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<CreateUpdateTaskItemDTO>> UpdateTaskItem(int id, CreateUpdateTaskItemDTO taskItem) {
            var validationResult = await _validator.ValidateAsync(new TaskItem
            {
                Title = taskItem.title,
                Description = taskItem.description,
                Status = taskItem.status,
                Priority = taskItem.priority,
                DueDate = taskItem.dueDate
            });
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _taskItemRepository.UpdateTaskItem(id, taskItem);

            if(result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<CreateUpdateTaskItemDTO>> UpdateTaskItemStatus(int id, Status status) {
            var validationResult = await _validator.ValidateAsync(new TaskItem
            {
                Status = status
            });
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

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
