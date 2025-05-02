using System.Threading.Tasks;
using TomAntillWebDevServices.Data.DataModels;

namespace TomAntillWebDevServices.Services.Contracts
{
    public interface IEmailService
    {
        public Task<string> Add(Email email, string websiteName);
        public Task<string> SendLogEmail(Email email, byte[] filebytes, string fileName);

    }
}
