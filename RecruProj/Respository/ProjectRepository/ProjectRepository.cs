using RecruProj.Data;
using RecruProj.Dtos.ProjectDtos;
using RecruProj.Dtos.TaskItemDtos;
using RecruProj.Models.ProjectsName;
using RecruProj.Models.TaskItemName;
using RecruProj.Models.Enums;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using RecruProj.Validators.GlobalExceptionHandler;

namespace RecruProj.Respository.ProjectRepository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public ProjectRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<CreateProjectsResponseDTO> CreateProject(CreateProjectsDTO project)
        {
            var existingproject = await _dbcontext.Projects.FirstOrDefaultAsync(p => p.Name == project.name);

            if (existingproject != null)
            {
                throw new ConflictException("Project with the same name already exists.");
            }
            var newProject = new Project
            {
                Name = project.name,
                Description = project.description,
                CreatedAt = DateTime.UtcNow
            };

            _dbcontext.Projects.Add(newProject);
            await _dbcontext.SaveChangesAsync();


            return new CreateProjectsResponseDTO(
                newProject.Id,
                newProject.Name,
                newProject.Description,
                newProject.CreatedAt
            );
        }

        public async Task<CreateUpdateTaskItemDTO> CreateTaskItem(int id, CreateUpdateTaskItemDTO taskItem)
        {
            var existingTask = await _dbcontext.TaskItems.Where(t => t.ProjectId == id).FirstOrDefaultAsync(t => t.Title == taskItem.title);

            if (existingTask != null)
            {
                throw new ConflictException("Task item with the same title already exists.");
            }
            var newTaskItem = new TaskItem
            {
                Title = taskItem.title,
                Description = taskItem.description,
                Status = taskItem.status,
                Priority = taskItem.priority,
                DueDate = taskItem.dueDate,
                CreatedAt = DateTime.UtcNow,
                ProjectId = id
            };

            _dbcontext.TaskItems.Add(newTaskItem);
            await _dbcontext.SaveChangesAsync();


            return taskItem;
        }

        public async Task<IEnumerable<GetProjectsDTO>> GetAllProjects()
        {
            var projects = await _dbcontext.Projects.Select(p => new GetProjectsDTO(
                p.Id,
                p.Name,
                p.Description,
                p.CreatedAt,
                p.Tasks.Count()
            )).ToListAsync();

            return projects;
        }

        public async Task<GetProjectsWithTaskDTO> GetAllTaskForProjectItems(int projectId)
        {
            var existingproject = await _dbcontext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if(existingproject == null)
            {
                throw new NotFoundException("Podane ID projektu nie istnieje.");
            }

            return new GetProjectsWithTaskDTO(
                existingproject.Id,
                existingproject.Name,
                existingproject.Description,
                existingproject.CreatedAt,
                await _dbcontext.TaskItems.Where(t => t.ProjectId == projectId).Select(t => new GetTaskItemDTO(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Status,
                    t.Priority,
                    t.DueDate
                )).ToListAsync()
            );

        }

        public async Task<GetTaskItemPagedDTO> GetProjectWithTaskFiltering(
            int projectId,
            Status? status,
            Priority? priority,
            int pageIndex,
            int pageSize)
        {
            var existingProject = await _dbcontext.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (existingProject == null)
            {
                throw new NotFoundException("Podane ID projektu nie istnieje.");
            }

            var query = _dbcontext.TaskItems
                .Where(x => x.ProjectId == projectId);

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (priority.HasValue)
                query = query.Where(x => x.Priority == priority.Value);

            var taskItems = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new GetTaskItemDTO(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Status,
                    t.Priority,
                    t.DueDate
                ))
                .ToListAsync();

            return new GetTaskItemPagedDTO(taskItems, pageIndex, pageSize);
        }

        public async Task<TasksSummaryDTO> GetTasksSummary(int projectId)
        {
            var existingproject = await _dbcontext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (existingproject == null)
            {
                throw new NotFoundException("Podane ID projektu nie istnieje.");
            }

            var totalTasks = await _dbcontext.TaskItems.CountAsync(t => t.ProjectId == projectId);
            var completedTasks = await _dbcontext.TaskItems.CountAsync(t => t.ProjectId == projectId && t.Status == Status.Done);
            var pendingTasks = await _dbcontext.TaskItems.CountAsync(t => t.ProjectId == projectId && t.Status == Status.ToDo);
            var inProgressTasks = await _dbcontext.TaskItems.CountAsync(t => t.ProjectId == projectId && t.Status == Status.InProgress);
            var percentageCompleted = totalTasks > 0 ? ((double)completedTasks / totalTasks * 100) : 0;

            return new TasksSummaryDTO(totalTasks, completedTasks, pendingTasks, inProgressTasks, percentageCompleted);
        }
}
}
