using RecruProj.Models.Enums;

namespace RecruProj.Dtos.TaskItemDtos;


    public record GetTaskItemDTO(int id, string title, string description, Status status, Priority priority, DateTime dueDate);
    public record GetTaskItemPagedDTO(IEnumerable<GetTaskItemDTO> tasks, int pageIndex, int pageSize, int totalCount);
    public record CreateUpdateTaskItemDTO(string title, string description, Status status, Priority priority, DateTime dueDate);
