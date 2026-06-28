using FluentValidation;
using RecruProj.Models.TaskItemName;
namespace RecruProj.Validators.TaskItemValidator
{
    public class TaskItemValidator : AbstractValidator<TaskItem>
    {
        public TaskItemValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty().WithMessage("Task title is required.")
                .MaximumLength(100).WithMessage("Task title cannot exceed 100 characters.");
            RuleFor(t => t.Description)
                .MaximumLength(500).WithMessage("Task description cannot exceed 500 characters.");
            RuleFor(t => t.DueDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Due date must be in the future.");
            RuleFor(t => t.Status)
                .IsInEnum().WithMessage("Invalid status value.");
            RuleFor(t => t.Priority)
                .IsInEnum().WithMessage("Invalid priority value.");
        }
    }
}
