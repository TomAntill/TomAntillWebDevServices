using TomAntillWebDevServices.Data.Enums;

namespace TomAntillWebDevServices.Models.ViewModels
{
    public class MediaVm
    {
        public MediaVm(int id, string name, string url, WebsiteName websiteName, UploadCategory pictureCategory, ProjectName projectName)
        {
            Id = id;
            Name = name;
            Url = url;
            WebsiteName = websiteName;
            PictureCategory = pictureCategory.ToString();
            ProjectName = projectName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public WebsiteName WebsiteName { get; set; }
        public string PictureCategory { get; set; }
        public ProjectName ProjectName { get; set; }

        public void TransfromPictureCategoryToProjectName()
        {
            PictureCategory = ProjectName.ToString();
        }
    }
}
