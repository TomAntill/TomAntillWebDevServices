using FluentValidation;

namespace TomAntillWebDevServices.Validation
{
    public class GetByIdValidator : AbstractValidator<int>
    {
        public GetByIdValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Id cannot be null");
            RuleFor(x => x).GreaterThanOrEqualTo(0).WithMessage("Id cannot be negative");
        }
    }
}