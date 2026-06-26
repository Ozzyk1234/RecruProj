using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecruProj.Dtos.ProjectDtos;
using RecruProj.Repository.TaskItemRepository;
using RecruProj.Respository.ProjectRepository;

namespace RecruProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectsController(IProjectRepository projectRepository, ITaskItemRepository taskItemRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProjectsDTO>>> GetAllProjects()
        {
            var projects = await _projectRepository.GetAllProjects();
            if(projects == null)
            {
                return NotFound();
            }

            return Ok(projects);
        }

        [HttpPost]
        public async Task<ActionResult<CreateProjectsDTO>> CreateProject(CreateProjectsDTO project)
        {
            var createproject = await _projectRepository.CreateProject(project);
            return Ok(createproject);
        }
    }
}
