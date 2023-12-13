using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TomAntillWebDevServices.Attributes;
using TomAntillWebDevServices.Data.Enums;
using TomAntillWebDevServices.Models.Commands;
using TomAntillWebDevServices.Models.ViewModels;
using TomAntillWebDevServices.Services.Contracts;
using TomAntillWebDevServices.Validation;
using TomAntillWebDevServices.Validation.Commands;

namespace TomAntillWebDevServices.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : JwtAuthController
    {
        private readonly IMediaService mediaService;

        public AdminController(IMediaService mediaService, IConfiguration configuration) : base (configuration)
        {
            this.mediaService = mediaService;
        }

        //add picture
        [HttpPost]
        [Route("AddImage")]
        [JwtAuthAttribute]
        public async Task<MediaVm> AddImage([FromForm] string name, [FromForm] IFormFile file
            , [FromForm] string websiteName, [FromForm] UploadCategory uploadCategory=UploadCategory.None, [FromForm] ProjectName projectName=ProjectName.None)
        {
            var command = new MediaAddCommand(name, file, websiteName, uploadCategory, projectName);
            MediaAddCommandValidator validator = new MediaAddCommandValidator();
            validator.ValidateAndThrow(command);
            return await mediaService.UploadMedia(command);
        }

        [HttpGet]
        [Route("GetAll")]
        [JwtAuthAttribute]
        public async Task<List<MediaVm>> GetAll(string appName, UploadCategory? category=null, ProjectName? projectName=null)
        {
            return await mediaService.GetAll(appName, category, projectName);
        }

        [HttpGet]
        [Route("GetById")]
        [JwtAuthAttribute]
        public async Task<MediaVm> GetById(string appName, int id)
        {
            GetByIdValidator validator = new GetByIdValidator();
            validator.ValidateAndThrow(id);
            return await mediaService.GetById(appName, id);
        }

        [HttpDelete]
        [Route("Delete")]
        [JwtAuthAttribute]
        public async Task<bool> Delete(string websiteName, int id)
        {
            GetByIdValidator validator = new GetByIdValidator();
            validator.ValidateAndThrow(id);
            await mediaService.DeleteMedia(websiteName, id);
            return true;
        }

        [HttpPost]
        [Route("Update")]
        [JwtAuthAttribute]
        public async Task<bool> Update([FromBody] MediaUpdateCommand command)
        {
            MediaUpdateCommandValidator validator = new MediaUpdateCommandValidator();
            validator.ValidateAndThrow(command);
            await mediaService.UpdateMediaData(command);
            return true;
        }       
    }
}
