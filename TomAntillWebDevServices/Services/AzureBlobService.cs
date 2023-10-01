using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomAntillWebDevServices.BLL.Contracts;
using TomAntillWebDevServices.Data;
using TomAntillWebDevServices.Data.Enums;
using TomAntillWebDevServices.Models.Commands;
using TomAntillWebDevServices.Models.ViewModels;
using TomAntillWebDevServices.Services.Contracts;

namespace TomAntillWebDevServices.Services
{
    public class AzureBlobService : IMediaService
    {
        private readonly IMediaBLL mediaBLL;
        private readonly BlobServiceClient blobServiceClient;
        private readonly AppDbContext appDbContext;

        public AzureBlobService(IMediaBLL mediaBLL, BlobServiceClient blobServiceClient, AppDbContext appDbContext)
        {
            this.mediaBLL = mediaBLL;
            this.blobServiceClient = blobServiceClient;
            this.appDbContext = appDbContext;
        }

        public async Task DeleteMedia(WebsiteName appName, int id)
        {
            //GET FILE NAME
            string fullFileName = appDbContext.BasePicture.FirstOrDefault(x => x.Id == id).Path;
            string fileName = Helpers.Helpers.TrimFileName(appName, fullFileName);

            //DELETE FILE
            BlobContainerClient cont = blobServiceClient.GetBlobContainerClient($"website-{appName}".ToLower());
            cont.GetBlobClient(fileName).DeleteIfExists();
            await mediaBLL.Delete(id);

            return;
        }
        public async Task<MediaVm> GetById(WebsiteName appName, int id)
        {
            return await mediaBLL.GetByIdAsync(id);
        }

        public async Task UpdateMediaData(MediaUpdateCommand command)
        {
            await mediaBLL.Update(command);
            return;
        }

        public async Task<MediaVm> UploadMedia(MediaAddCommand command)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient($"website-{command.WebsiteName}".ToLower());
            await containerClient.CreateIfNotExistsAsync(publicAccessType: Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);

            var blobClient = containerClient.GetBlobClient(GetBlobUploadName(command.File.FileName));

            using var stream = command.File.OpenReadStream();

            var blobInfo = await blobClient.UploadAsync(stream);

            command.Url = blobClient.Uri.AbsoluteUri;

            return await mediaBLL.Add(command);
        }

        public async Task<List<MediaVm>> GetAll(WebsiteName appName, UploadCategory? category = null, ProjectName? projectName = null)
        {
            return await mediaBLL.GetAllAsync(appName, category, projectName);
        }

        private static string GetBlobUploadName(string fileName)
        {
            return string.Format("{1}-{0}", fileName, Guid.NewGuid());
        }
    }
}
