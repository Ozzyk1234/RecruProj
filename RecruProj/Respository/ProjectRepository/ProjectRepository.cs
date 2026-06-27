using RecruProj.Data;
using RecruProj.Dtos.ProjectDtos;
using RecruProj.Dtos.TaskItemDtos;
using RecruProj.Models.ProjectsName;
using RecruProj.Models.TaskItemName;
using RecruProj.Models.Enums;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace RecruProj.Respository.ProjectRepository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public ProjectRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<CreateProjectsDTO> CreateProject(CreateProjectsDTO project)
        {
            var existingproject = await _dbcontext.Projects.FirstOrDefaultAsync(p => p.Name == project.name);

            if (existingproject != null)
            {
                throw new Exception("Project with the same name already exists.");
            }
            var newProject = new Project
            {
                Name = project.name,
                Description = project.description,
                CreatedAt = DateTime.UtcNow
            };

            _dbcontext.Projects.Add(newProject);
            await _dbcontext.SaveChangesAsync();


            return project;
        }

        public async Task<CreateUpdateTaskItemDTO> CreateTaskItem(int id, CreateUpdateTaskItemDTO taskItem)
        {
            var existingTask = await _dbcontext.TaskItems.FirstOrDefaultAsync(t => t.Title == taskItem.title);

            if (existingTask != null)
            {
                throw new Exception("Task item with the same title already exists.");
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

        public async Task<IEnumerable<GetProjectsWithTaskDTO>> GetAllTaskForProjectItems(int projectId)
        {
            var existingproject = await _dbcontext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if(existingproject == null)
            {
                throw new Exception("Podane ID projektu nie istnieje.");
            }

            var projectwithtaskitems = await _dbcontext.Projects.Where(x => x.Id == projectId).Select(t => new GetProjectsWithTaskDTO(
                t.Id,
                t.Name,
                t.Description,
                t.CreatedAt,
                t.Tasks.Select(tasks => new GetTaskItemDTO(
                    tasks.Id,
                    tasks.Title,
                    tasks.Description,
                    tasks.Status,
                    tasks.Priority,
                    tasks.DueDate
                ))
            )).ToListAsync();

            return projectwithtaskitems;
        }

        public async Task<IEnumerable<GetTaskItemDTO>> GetProjectWithTaskFiltering(int projectId, Status? status, Priority? priority)
        {
            var existingproject = await _dbcontext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (existingproject == null)
            {
                throw new Exception("Podane ID projektu nie istnieje.");
            }

            if(status != null && priority != null)
            {
                var TaskItems = await _dbcontext.TaskItems.Where(x => x.ProjectId == projectId && x.Status == status && x.Priority == priority).Select(t => new GetTaskItemDTO(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Status,
                    t.Priority,
                    t.DueDate
                )).ToListAsync();
                return TaskItems;
            }
            else if(status != null)
            {
                var TaskItems = await _dbcontext.TaskItems.Where(x => x.ProjectId == projectId && x.Status == status).Select(t => new GetTaskItemDTO(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Status,
                    t.Priority,
                    t.DueDate
                )).ToListAsync();
                return TaskItems;
            }
            else if(priority != null)
            {
                var TaskItems = await _dbcontext.TaskItems.Where(x => x.ProjectId == projectId && x.Priority == priority).Select(t => new GetTaskItemDTO(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Status,
                    t.Priority,
                    t.DueDate
                )).ToListAsync();
                return TaskItems;
            }
            else
            {
                throw new Exception("Invalid filter values.");
            }
        }

        public async Task<TasksSummaryDTO> GetTasksSummary(int projectId)
        {
            var existingproject = await _dbcontext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (existingproject == null)
            {
                throw new Exception("Podane ID projektu nie istnieje.");
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
