using System.Collections.Generic;
using System.Threading.Tasks;
using TomAntillWebDevServices.Data.Enums;
using TomAntillWebDevServices.Models.ViewModels;

namespace TomAntillWebDevServices.Data.Contracts
{
    public interface IBaseDAL<T, TUpdate, TAdd>
    {
        public Task<MediaVm> Add(TAdd t);
        public Task<bool> Update(TUpdate t);
        public Task<bool> Delete(int id);
        public Task<T> GetById(int id);
        public Task<List<T>> GetAll(string appName, UploadCategory? category = null, ProjectName? projectName = null);
    }
}
