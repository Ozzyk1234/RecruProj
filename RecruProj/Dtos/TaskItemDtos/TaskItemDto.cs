using RecruProj.Models.Enums;

namespace RecruProj.Dtos.TaskItemDtos;


    public record GetTaskItemDTO(int id, string title, string description, Status status, Priority priority, DateTime dueDate);
    public record GetTaskItemPagedDTO(IEnumerable<GetTaskItemDTO> tasks, int pageIndex, int pageSize, float totalCount);
    public record PaginationValidatorDTO(int pageIndex, int pageSize);
    public record CreateUpdateTaskItemDTO(string title, string description, Status status, Priority priority, DateTime dueDate);
