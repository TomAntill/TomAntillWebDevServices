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

        private string SelectEmailFrom(WebsiteName websiteName)
        {
            string websiteEmail = null;
            if (websiteName == WebsiteName.CoatesCarpentry)
            {
                websiteEmail = "info@coatescarpentry.co.uk";
            }
            else if (websiteName == WebsiteName.Portfolio)
            {
                //will need updating
                websiteEmail = "enquiries@coatescarpentry.co.uk";
            }
            return websiteEmail;
        }


        public EmailService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }
        public async Task<string> Add(Email email, WebsiteName websiteName)
        {
            EmailValidator validator = new EmailValidator();
            validator.ValidateAndThrow(email);
            // Retrieve the SendGrid API key from the environment variable
            var apiKey = Environment.GetEnvironmentVariable("SEND_GRID_API_KEY");

            // Create a SendGrid client using the API key
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(SelectEmailFrom(websiteName), email.EmailAddress);
            var details = GetEmailDetails(websiteName);
            var dynamicData = new { from = email.EmailAddress, htmlcontent = email.Message, name = email.Name };
            var msg = MailHelper.CreateSingleTemplateEmail(from, details.EmailAddress, details.TemplateId, dynamicData);
            var res = await client.SendEmailAsync(msg);
            

            email.EmailSettingsId = details.EmailSettingsId;
            _context.Email.Add(email);
            await _context.SaveChangesAsync();
            return $"{res?.StatusCode} - {res?.IsSuccessStatusCode}";
        }

        private EmailFromModel GetEmailDetails(WebsiteName websiteName)
        {
            var email = _context.EmailSettings.FirstOrDefault(x => x.WebsiteName == websiteName);

            EmailFromModel details = new EmailFromModel(email.EmailAddress, email.Template, email.Id);
            return details;
        }
    }
}
