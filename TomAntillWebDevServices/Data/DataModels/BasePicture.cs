using Microsoft.EntityFrameworkCore;
using System;
using TomAntillWebDevServices.Data.Enums;

namespace TomAntillWebDevServices.Data.DataModels
{
    public class BasePicture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public WebsiteName WebsiteName { get; set; }
        public UploadCategory PictureCategory { get; set; }
        public ProjectName ProjectName { get; set; }

        public static void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<BasePicture>(); 
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.WebsiteName).IsRequired().HasConversion(v => v.ToString(),
                                                                            v => (WebsiteName)Enum.Parse(typeof(WebsiteName), v));
            entity.Property(e => e.PictureCategory).HasConversion(v => v.ToString(),
                                                                            v => (UploadCategory)Enum.Parse(typeof(UploadCategory), v));
            entity.Property(e => e.ProjectName).HasConversion(v => v.ToString(),
                                                                            v => (ProjectName)Enum.Parse(typeof(ProjectName), v));
        }
    }
}
