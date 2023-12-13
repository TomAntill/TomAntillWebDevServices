using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TomAntillWebDevServices.BLL.Contracts;
using TomAntillWebDevServices.Data.Contracts;
using TomAntillWebDevServices.Data.Enums;
using TomAntillWebDevServices.Models.Commands;
using TomAntillWebDevServices.Models.ViewModels;

namespace TomAntillWebDevServices.BLL
{
    public class MediaBLL : IMediaBLL
    {
        private IMediaDAL _pictureDAL = null;

        public MediaBLL(IBaseDAL<MediaVm, MediaUpdateCommand, MediaAddCommand> pictureDAL)
        {
            _pictureDAL = (IMediaDAL)(pictureDAL ?? throw new ArgumentNullException(nameof(pictureDAL)));
        }
        public async Task<MediaVm> Add(MediaAddCommand command)
        {
            return await _pictureDAL.Add(command);
        }
        public async Task<List<MediaVm>> GetAllAsync(string appName, UploadCategory? category = null, ProjectName? projectName = null)
        {
            return await _pictureDAL.GetAll(appName, category, projectName);

        }
        public async Task<MediaVm> GetByIdAsync(int id) => await _pictureDAL.GetById(id);
        public async Task<bool> Delete(int id) => await _pictureDAL.Delete(id);
        public async Task<bool> Update(MediaUpdateCommand command)
        {
            return await _pictureDAL.Update(command);
        }
    }
}
