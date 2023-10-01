using Microsoft.EntityFrameworkCore;
using System;
using TomAntillWebDevServices.Data.Enums;

namespace TomAntillWebDevServices.Data.DataModels
{
    public class EmailSettings
    {
        public int Id { get; set; }
        public string Template { get; set; }
        public WebsiteName WebsiteName { get; set; }
        public string EmailAddress { get; set; }

        public static void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<EmailSettings>();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.WebsiteName).HasConversion(v => v.ToString(),
                                                                            v => (WebsiteName)Enum.Parse(typeof(WebsiteName), v));
        }
    }
}
