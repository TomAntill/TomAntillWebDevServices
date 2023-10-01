using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.IO;
using TomAntillWebDevServices.Models.Commands;

namespace TomAntillWebDevServices.Validation.Commands
{
    public class MediaAddCommandValidator : AbstractValidator<MediaAddCommand>
    {
        public MediaAddCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name cannot be null");
            RuleFor(x => x.File).NotNull().WithMessage("File cannot be null");
            RuleFor(x => x.Name.Length).LessThan(100).WithMessage("File name too long").When(x => x.Name is not null);
            RuleFor(x => x.File.Length).LessThan(25000000).WithMessage("Invalid file size").When(x => x.File is not null);
            RuleFor(x => x.File).SetValidator(new FileValidator()).When(x => x is not null);
        }
    }
    public class FileValidator : AbstractValidator<IFormFile>
    {
        public FileValidator()
        {
            RuleFor(x => x.Length).LessThan(25000000).WithMessage("Invalid file size");
            RuleFor(x => x.FileName).Must(BeAValidExtension).WithMessage("Invalid file extension");
        }
        private bool BeAValidExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (extension == ".jpeg" || extension == ".png"  || extension == ".jpg" || extension == ".avif" || extension == ".webp"
                || extension == ".heic" || extension == ".heif")
                return true;
            else
                return false;
        }
    }
}
