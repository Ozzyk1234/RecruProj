using FluentValidation;
using RecruProj.Dtos.TaskItemDtos;

namespace RecruProj.Validators.TaskItemValidator
{
    public class PaginationValidator : AbstractValidator<PaginationValidatorDTO>
    {
        public PaginationValidator()
        {
            RuleFor(p => p.pageIndex)
                .GreaterThanOrEqualTo(1).WithMessage("Page index must be greater than or equal to 0.");
            RuleFor(p => p.pageSize)
                .GreaterThanOrEqualTo(1).WithMessage("Page size must be a positive integer.");
        }
    }
}
