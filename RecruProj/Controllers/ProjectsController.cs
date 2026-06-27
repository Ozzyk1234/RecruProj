using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RecruProj.Dtos.ProjectDtos;
using RecruProj.Dtos.TaskItemDtos;
using RecruProj.Models.Enums;
using RecruProj.Models.ProjectsName;
using RecruProj.Repository.TaskItemRepository;
using RecruProj.Respository.ProjectRepository;

namespace RecruProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
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
            var result = await _projectRepository.CreateProject(project);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("{projectId}/tasks")]
        public async Task<ActionResult<IEnumerable<GetTaskItemDTO>>> GetProjectWithTaskFiltering(int projectId, [FromQuery] Status? status, [FromQuery] Priority? priority)
        {
            var result = await _projectRepository.GetProjectWithTaskFiltering(projectId, status, priority);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<IEnumerable<GetTaskItemDTO>>> GetAllTaskForProjectItems(int projectId)
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
