using System.Collections.Generic;
using System.Threading.Tasks;
using TomAntillWebDevServices.Data.Enums;
using TomAntillWebDevServices.Models.Commands;
using TomAntillWebDevServices.Models.ViewModels;

namespace TomAntillWebDevServices.Services.Contracts
{
    public interface IMediaService
    {
        public Task DeleteMedia(WebsiteName appName, int id);
        public Task UpdateMediaData(MediaUpdateCommand command);
        public Task<List<MediaVm>> GetAll(WebsiteName appName, UploadCategory? category = null, ProjectName? projectName = null);
        public Task<MediaVm> GetById(WebsiteName appName, int id);
        public Task<MediaVm> UploadMedia(MediaAddCommand command);
    }
}
