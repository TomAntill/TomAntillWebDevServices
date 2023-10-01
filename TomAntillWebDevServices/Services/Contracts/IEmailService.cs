using System.Threading.Tasks;
using TomAntillWebDevServices.Data.DataModels;
using TomAntillWebDevServices.Data.Enums;

namespace TomAntillWebDevServices.Services.Contracts
{
    public interface IEmailService
    {
        public Task<string> Add(Email email, WebsiteName websiteName);
    }
}
