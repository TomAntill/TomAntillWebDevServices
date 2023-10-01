using System.Collections.Generic;
using System.Threading.Tasks;
using TomAntillWebDevServices.Data.Enums;

namespace TomAntillWebDevServices.BLL.Contracts
{
    public interface IBaseBLL<T, TUpdate, TAdd>
    {
        public Task<T> Add(TAdd t);
        public Task<bool> Delete(int id);
        public Task<bool> Update(TUpdate t);
        public Task<T> GetByIdAsync(int id);
        public Task<List<T>> GetAllAsync(WebsiteName appName, UploadCategory? category = null, ProjectName? projectName = null);


    }
}
