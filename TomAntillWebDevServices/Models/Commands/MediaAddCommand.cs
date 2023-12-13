using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TomAntillWebDevServices.Data.Enums;

namespace TomAntillWebDevServices.Models.Commands
{
    public class BaseMediaCommand
    {
        public BaseMediaCommand(string name, string websiteName, UploadCategory uploadCategory, ProjectName projectName)
        {
            Name = name;
            WebsiteName = websiteName;
            UploadCategory = uploadCategory;
            ProjectName = projectName;
        }

        public string Name { get; set; }
        public string WebsiteName { get; set; }
        public UploadCategory UploadCategory { get; set; }
        public ProjectName ProjectName { get; set; }
    }

    public class MediaUpdateCommand : BaseMediaCommand
    {
        [JsonConstructor]
        public MediaUpdateCommand(int id, string name, string websiteName, UploadCategory uploadCategory, ProjectName projectName)
            : base(name, websiteName, uploadCategory, projectName)
        {
            Id = id;
        }   

        public int Id { get; set; }
    }

    public class MediaAddCommand : BaseMediaCommand
    {
        public MediaAddCommand(string name, IFormFile file, string websiteName, UploadCategory uploadCategory, ProjectName projectName)
            : base(name, websiteName, uploadCategory, projectName)
        {
            Name = name;
            File = file;
            WebsiteName = websiteName;
            UploadCategory = uploadCategory;
            ProjectName = projectName;
        }

        public IFormFile File { get; set; }
        public string Url { get; set; }
    }
}
