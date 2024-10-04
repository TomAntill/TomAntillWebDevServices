using FluentValidation;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;
using System.Threading.Tasks;
using TomAntillWebDevServices.Data;
using TomAntillWebDevServices.Data.DataModels;
using TomAntillWebDevServices.Data.Enums;
using TomAntillWebDevServices.Services.Contracts;
using TomAntillWebDevServices.Validation;
using static TomAntillWebDevServices.Helpers.HelperModels;

namespace TomAntillWebDevServices.Services
{
    public class EmailService : IEmailService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration configuration;

        public EmailService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        public async Task<string> Add(Email email, string websiteName)
        {
            ValidateEmail(email);
            var client = GetSendGridClient();

            var from = new EmailAddress(SelectEmailFrom(websiteName), email.EmailAddress);
            var sendTo = GetEmailDetails(websiteName);
            var emailContent = new { from = email.EmailAddress, htmlcontent = email.Message, name = email.Name };

            var msg = MailHelper.CreateSingleTemplateEmail(from, sendTo.EmailAddress, sendTo.TemplateId, emailContent);
            var res = await client.SendEmailAsync(msg);

            email.EmailSettingsId = sendTo.EmailSettingsId;

            if (websiteName != "LeahSLT") {
                _context.Email.Add(email);
                await _context.SaveChangesAsync();
            }

            return $"{res?.StatusCode} - {res?.IsSuccessStatusCode}";
        }

        private static void ValidateEmail(Email email)
        {
            EmailValidator validator = new();
            validator.ValidateAndThrow(email);
        }

        private static SendGridClient GetSendGridClient()
        {
            // Retrieve the SendGrid API key from the environment variable
            var apiKey = Environment.GetEnvironmentVariable("SEND_GRID_API_KEY");

            // Create a SendGrid client using the API key
            return new SendGridClient(apiKey);
        }

        private static string SelectEmailFrom(string websiteCode)
        {
            switch (websiteCode)
            {
                case nameof(Website.CoatesCarpentry):
                    return "info@coatescarpentry.co.uk";
                case nameof(Website.TidyElectrics):
                    //will need updating
                    return "info@coatescarpentry.co.uk";
                case nameof(Website.Portfolio):
                    //will need updating
                    return "enquiries@coatescarpentry.co.uk";
                case nameof(Website.LeahSLT):
                    return "info@leahslt.co.uk";
                default:
                    return "info@coatescarpentry.co.uk";
            }
        }

        private SendToModel GetEmailDetails(string websiteName)
        {
            var email = _context.EmailSettings.FirstOrDefault(x => x.WebsiteName == websiteName);
            SendToModel details = new(email.EmailAddress, email.Template, email.Id);
            return details;
        }
    }
}
