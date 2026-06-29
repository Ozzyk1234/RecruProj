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
        public Task<CreateProjectsResponseDTO> CreateProject(CreateProjectsDTO project);
        public Task<GetTaskItemPagedDTO> GetProjectWithTaskFiltering(int projectId, Status? status, Priority? priority, int pageIndex, int pageSize);
        public Task<GetProjectsWithTaskDTO> GetAllTaskForProjectItems(int projectId);
        public Task<CreateUpdateTaskItemDTO> CreateTaskItem(int projectId, CreateUpdateTaskItemDTO taskItem);
        public Task<TasksSummaryDTO> GetTasksSummary(int projectId);
    }
}
