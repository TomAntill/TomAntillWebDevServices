using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using TomAntillWebDevServices.Data.DataModels;

namespace TomAntillWebDevServices.BLL.Contracts
{
    public interface IEmailBLL
    {
        public Task<string> Add(Email email, string websiteName);

        public Task<string> SendLogEmail(StringValues email, IFormFile file);

    }
}
