using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomAntillWebDevServices.Data.Contracts;
using TomAntillWebDevServices.Data.DataModels;
using TomAntillWebDevServices.Data.Enums;
using TomAntillWebDevServices.Models.Commands;
using TomAntillWebDevServices.Models.ViewModels;

namespace TomAntillWebDevServices.Data
{
    public class MediaDAL : IMediaDAL
    {
        private AppDbContext _context = null;
        public MediaDAL(AppDbContext dbContext)
        {
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<MediaVm> Add(MediaAddCommand command)
        {


            //add details to database
            var picture = new BasePicture
            {
                Name = command.Name,
                PictureCategory = command.UploadCategory,
                ProjectName = command.ProjectName,
                Path = command.Url,
                WebsiteName = command.WebsiteName,
            };

            
            _context.BasePicture.Add(picture);
            await _context.SaveChangesAsync();

            return new MediaVm(picture.Id, picture.Name, picture.Path, picture.WebsiteName, picture.PictureCategory, picture.ProjectName);
        }

        public async Task<bool> Update(MediaUpdateCommand updatedMdl)
        {

            BasePicture picture = _context.BasePicture.FirstOrDefault(x => x.Id == updatedMdl.Id);

            //Update database details
            picture.Name = updatedMdl.Name;
            picture.PictureCategory = updatedMdl.UploadCategory;
            picture.ProjectName = updatedMdl.ProjectName;
            picture.WebsiteName = updatedMdl.WebsiteName;

            int updated = await _context.SaveChangesAsync();

            return updated > 0;
        }
        public async Task<bool> Delete(int id)
        {
            //get picture details
            BasePicture media = _context.BasePicture.FirstOrDefault(x => x.Id == id);
            if (media == null) return false;

            //delete database entry
            _context.BasePicture.Remove(media);
            int deleted = await _context.SaveChangesAsync();

            return true;
        }
        public async Task<MediaVm> GetById(int id)
        {
            BasePicture media = await _context.BasePicture.FirstOrDefaultAsync(x => x.Id == id);

            MediaVm mediaVm = new MediaVm(media.Id, media.Name, media.Path, media.WebsiteName, media.PictureCategory, media.ProjectName);

            return mediaVm;
        }

        public async Task<List<MediaVm>> GetAll(string appName, UploadCategory? category = null, ProjectName? projectName = null)
        {
            IQueryable<BasePicture> query = _context.BasePicture.Where(vm => vm.WebsiteName == appName);

            if (category is not null)
                query = query.Where(q => q.PictureCategory == category.Value);
            if (projectName is not null)
                query = query.Where(q => q.ProjectName == projectName.Value);

            return await query.Select(s => new MediaVm(s.Id, s.Name, s.Path, s.WebsiteName, s.PictureCategory, s.ProjectName)).ToListAsync();
        }
    }
}
