using FluentValidation;
using RecruProj.Dtos.TaskItemDtos;
using RecruProj.Models.TaskItemName;
namespace RecruProj.Validators.TaskItemValidator
{
    public class TaskItemValidator : AbstractValidator<CreateUpdateTaskItemDTO>
    {
        public TaskItemValidator()
        {
            RuleFor(t => t.title)
                .NotEmpty().WithMessage("Task title is required.")
                .MaximumLength(100).WithMessage("Task title cannot exceed 100 characters.");
            RuleFor(t => t.description)
                .MaximumLength(500).WithMessage("Task description cannot exceed 500 characters.");
            RuleFor(t => t.dueDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Due date must be in the future.");
            RuleFor(t => t.status)
                .IsInEnum().WithMessage("Invalid status value.");
            RuleFor(t => t.priority)
                .IsInEnum().WithMessage("Invalid priority value.");
        }
    }
}
