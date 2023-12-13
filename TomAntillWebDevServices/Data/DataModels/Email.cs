using Microsoft.EntityFrameworkCore;
using System;
using TomAntillWebDevServices.Data.Enums;

namespace TomAntillWebDevServices.Data.DataModels
{
    public class Email
    {
        public int Id { get; set; }
        public int EmailSettingsId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Message { get; set; }
        public string WebsiteName { get; set; }
        public virtual EmailSettings EmailSettings { get; set; }

        public static void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Email>();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.EmailAddress).IsRequired();
            entity.Property(e => e.WebsiteName).IsRequired();
            entity.HasOne(e => e.EmailSettings).WithMany().HasForeignKey(v => v.EmailSettingsId);
        }
    }
}
