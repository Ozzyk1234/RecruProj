using RecruProj.Models.Enums;
using RecruProj.Models.ProjectsName;
namespace RecruProj.Models.TaskItemName
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

    }
}
