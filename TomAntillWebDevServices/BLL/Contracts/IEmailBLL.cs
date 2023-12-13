using System.Threading.Tasks;
using TomAntillWebDevServices.Data.DataModels;

namespace TomAntillWebDevServices.BLL.Contracts
{
    public interface IEmailBLL
    {
        public Task<string> Add(Email email, string websiteName);

    }
}
