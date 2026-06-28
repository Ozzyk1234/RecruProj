using FluentValidation;
using RecruProj.Models.ProjectsName;
namespace RecruProj.Validators.ProjectValidator
{
    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator() { 
            
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Project name is required.")
                .MaximumLength(100).WithMessage("Project name cannot exceed 100 characters.");
            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("Project description cannot exceed 500 characters.");

        }
    }
}
