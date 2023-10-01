using System.Collections.Generic;
using System.Threading.Tasks;
using TomAntillWebDevServices.Data.Enums;
using TomAntillWebDevServices.Models.Commands;
using TomAntillWebDevServices.Models.ViewModels;

namespace TomAntillWebDevServices.BLL.Contracts
{
    public interface IMediaBLL : IBaseBLL<MediaVm, MediaUpdateCommand, MediaAddCommand>
    {
    }
}
