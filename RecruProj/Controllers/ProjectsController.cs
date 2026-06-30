using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RecruProj.Dtos.ProjectDtos;
using RecruProj.Dtos.TaskItemDtos;
using RecruProj.Models.Enums;
using RecruProj.Models.ProjectsName;
using RecruProj.Models.TaskItemName;
using RecruProj.Repository.TaskItemRepository;
using RecruProj.Respository.ProjectRepository;

namespace RecruProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IValidator<Project> _ProjectValidator;
        private readonly IValidator<TaskItem> _TaskItemValidator;

        public ProjectsController(IProjectRepository projectRepository, IValidator<Project> projectValidator, IValidator<TaskItem> taskItemValidator)
        {
            _projectRepository = projectRepository;
            _ProjectValidator = projectValidator;
            _TaskItemValidator = taskItemValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProjectsDTO>>> GetAllProjects()
        {
            var result = await _projectRepository.GetAllProjects();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CreateProjectsDTO>> CreateProject([FromBody] CreateProjectsDTO project)
        {
            var validationResult = await _ProjectValidator.ValidateAsync(project);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _projectRepository.CreateProject(project);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("{projectId}/tasks")]
        public async Task<ActionResult<IEnumerable<GetTaskItemDTO>>> GetProjectWithTaskFiltering(int projectId, [FromQuery] Status? status, [FromQuery] Priority? priority, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _projectRepository.GetProjectWithTaskFiltering(projectId, status, priority, pageIndex, pageSize);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<GetProjectsWithTaskDTO>> GetAllTaskForProjectItems(int projectId)
        {
            var result = await _projectRepository.GetAllTaskForProjectItems(projectId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost("{projectId}/tasks")]
        public async Task<ActionResult<CreateUpdateTaskItemDTO>> CreateTaskItem(int projectId, [FromBody] CreateUpdateTaskItemDTO taskItem)
        {
            var validationResult = await _TaskItemValidator.ValidateAsync(new TaskItem
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

            var result = await _projectRepository.CreateTaskItem(projectId, taskItem);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{projectId}/summary")]
        public async Task<ActionResult<TasksSummaryDTO>> GetProjectSummary(int projectId)
        {
            var result = await _projectRepository.GetTasksSummary(projectId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}

