using System.Collections.Generic;
using System.Threading.Tasks;
using TomAntillWebDevServices.Data.Enums;
using TomAntillWebDevServices.Models.Commands;
using TomAntillWebDevServices.Models.ViewModels;

namespace TomAntillWebDevServices.Data.Contracts
{
    public interface IMediaDAL : IBaseDAL<MediaVm, MediaUpdateCommand, MediaAddCommand>
    {
    }
}
