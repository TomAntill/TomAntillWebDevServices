using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TomAntillWebDevServices.BLL.Contracts;
using TomAntillWebDevServices.Data.DataModels;
using TomAntillWebDevServices.Services.Contracts;


namespace TomAntillWebDevServices.BLL
{
    public class EmailBLL : IEmailBLL
    {
        private IEmailService _emailService = null;

        public EmailBLL(IEmailService emailService)
        {
            _emailService = (IEmailService)(emailService ?? throw new ArgumentNullException(nameof(emailService)));
        }
        public async Task<string> Add(Email email, string websiteName)
        {
            return await _emailService.Add(email, websiteName);
        }

        public async Task<string> SendLogEmail(StringValues emailDataString, IFormFile file)
        {
            Email email;
            try
            {
                email = JsonSerializer.Deserialize<Email>(emailDataString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid email data format: " + ex.Message);
            }
            if (email == null)
                throw new Exception("Invalid email data format");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            return await _emailService.SendLogEmail(email, fileBytes, file.FileName);
        }

    }
}
