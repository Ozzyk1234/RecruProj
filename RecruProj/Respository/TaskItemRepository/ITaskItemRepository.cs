using RecruProj.Dtos.TaskItemDtos;
using RecruProj.Models.TaskItemName;
using RecruProj.Models.Enums;

namespace RecruProj.Repository.TaskItemRepository;

public interface ITaskItemRepository
{
    public Task<CreateUpdateTaskItemDTO> UpdateTaskItem(int id, CreateUpdateTaskItemDTO taskItem);
    public Task<CreateUpdateTaskItemDTO> UpdateTaskItemStatus(int id, Status status);
    public Task<bool> DeleteTaskItem(int id);

}
