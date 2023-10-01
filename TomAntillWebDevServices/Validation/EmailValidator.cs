using FluentValidation;
using TomAntillWebDevServices.Data.DataModels;

namespace TomAntillWebDevServices.Validation
{
    public class EmailValidator : AbstractValidator<Email>
    {
        public EmailValidator()
        {
            RuleFor(x => x.EmailAddress).NotNull().WithMessage("Email cannot be null");
            RuleFor(x => x.EmailAddress).EmailAddress().WithMessage("Please Provide Valid Email Address");
        }
    }
}
