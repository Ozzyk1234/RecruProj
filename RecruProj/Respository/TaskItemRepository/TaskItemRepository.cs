using Microsoft.EntityFrameworkCore;
using RecruProj.Data;
using RecruProj.Dtos.TaskItemDtos;
using RecruProj.Models.Enums;
using RecruProj.Models.TaskItemName;
using RecruProj.Repository.TaskItemRepository;
using RecruProj.Validators.GlobalExceptionHandler;

namespace RecruProj.Respository.TaskItemRepository
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public TaskItemRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<bool> DeleteTaskItem(int id)
        {
            var deletingitem = await _dbcontext.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
            if (deletingitem == null)
            {
                throw new NotFoundException("Podane ID nie istnieje.");
            }

            _dbcontext.TaskItems.Remove(deletingitem);
            await _dbcontext.SaveChangesAsync();

            return true;
        }

        public async Task<CreateUpdateTaskItemDTO> UpdateTaskItem(int id, CreateUpdateTaskItemDTO taskItem)
        {
            var findTask = await _dbcontext.TaskItems.FirstOrDefaultAsync(task => task.Id == id);

            if(findTask == null)
            {
                throw new NotFoundException("Podane ID nie istnieje.");
            }

            findTask.Title = taskItem.title;
            findTask.Description = taskItem.description;
            findTask.Status = taskItem.status;
            findTask.Priority = taskItem.priority;
            findTask.DueDate = taskItem.dueDate;

            await _dbcontext.SaveChangesAsync();

            return new CreateUpdateTaskItemDTO(findTask.Title, findTask.Description, findTask.Status, findTask.Priority, findTask.DueDate);
        }

        public async Task<CreateUpdateTaskItemDTO> UpdateTaskItemStatus(int id, Status status)
        {
            var findTask = await _dbcontext.TaskItems.FirstOrDefaultAsync(task => task.Id == id);

            if (findTask == null)
            {
                throw new NotFoundException("Podane ID nie istnieje.");
            }

            findTask.Status = status;

            await _dbcontext.SaveChangesAsync();

            return new CreateUpdateTaskItemDTO(findTask.Title, findTask.Description, findTask.Status, findTask.Priority, findTask.DueDate);
        }
    }
}
