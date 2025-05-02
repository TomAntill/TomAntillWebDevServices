using System;
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

        public async Task<string> SendLogEmail(Email email, byte[] filebytes, string fileName)
        {
            return await _emailService.SendLogEmail(email, filebytes, fileName);
        }
    }
}
