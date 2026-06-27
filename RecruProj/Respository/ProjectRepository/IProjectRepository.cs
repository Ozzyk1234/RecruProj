using RecruProj.Dtos.ProjectDtos;
using RecruProj.Dtos.TaskItemDtos;
using RecruProj.Models.TaskItemName;

using RecruProj.Models.ProjectsName;
using RecruProj.Models.Enums;

namespace RecruProj.Respository.ProjectRepository
{
    public interface IProjectRepository
    {
        public Task<IEnumerable<GetProjectsDTO>> GetAllProjects();
        public Task<CreateProjectsDTO> CreateProject(CreateProjectsDTO project);
        public Task<IEnumerable<GetTaskItemDTO>> GetProjectWithTaskFiltering(int projectId, Status? status, Priority? priority);
        public Task<IEnumerable<GetProjectsWithTaskDTO>> GetAllTaskForProjectItems(int projectId);
        public Task<CreateUpdateTaskItemDTO> CreateTaskItem(int projectId, CreateUpdateTaskItemDTO taskItem);
        public Task<TasksSummaryDTO> GetTasksSummary(int projectId);
    }
}
