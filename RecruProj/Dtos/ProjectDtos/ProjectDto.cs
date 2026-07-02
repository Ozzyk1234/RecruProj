using RecruProj.Dtos.TaskItemDtos;
using RecruProj.Models.TaskItemName;

namespace RecruProj.Dtos.ProjectDtos;

    public record GetProjectsDTO(int id, string name, string description, DateTime createdAt, int taskCount);
    public record GetProjectsWithTaskDTO(int id, string name, string description, DateTime createdAt, IEnumerable<GetTaskItemDTO> tasks);
    public record CreateProjectsDTO(string name, string description);
    public record CreateProjectsResponseDTO(int id, string name, string description, DateTime createdAt);

    public record TasksSummaryDTO(int totalTasks, int completedTasks, int pendingTasks, int inProgressTasks, double percentageCompleted);
