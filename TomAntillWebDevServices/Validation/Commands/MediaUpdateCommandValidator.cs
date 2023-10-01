using FluentValidation;
using TomAntillWebDevServices.Models.Commands;

namespace TomAntillWebDevServices.Validation.Commands
{
    public class MediaUpdateCommandValidator : AbstractValidator<MediaUpdateCommand>
    {
        public MediaUpdateCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name cannot be null");
            RuleFor(x => x.Name.Length).LessThan(100).WithMessage("File name too long").When(x => x.Name is not null);
        }
    }
}
