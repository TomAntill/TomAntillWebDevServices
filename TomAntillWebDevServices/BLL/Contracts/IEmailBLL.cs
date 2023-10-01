using System.Threading.Tasks;
using TomAntillWebDevServices.Data.DataModels;
using TomAntillWebDevServices.Data.Enums;

namespace TomAntillWebDevServices.BLL.Contracts
{
    public interface IEmailBLL
    {
        public Task<string> Add(Email email, WebsiteName websiteName);

    }
}
