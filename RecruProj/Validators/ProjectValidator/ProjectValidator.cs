using FluentValidation;
using RecruProj.Dtos.ProjectDtos;
using RecruProj.Models.ProjectsName;
namespace RecruProj.Validators.ProjectValidator
{
    public class ProjectValidator : AbstractValidator<CreateProjectsDTO>
    {
        public ProjectValidator() { 
            
            RuleFor(p => p.name)
                .NotEmpty().WithMessage("Project name is required.")
                .MaximumLength(100).WithMessage("Project name cannot exceed 100 characters.");
            RuleFor(p => p.description)
                .MaximumLength(500).WithMessage("Project description cannot exceed 500 characters.");

        }
    }
}
