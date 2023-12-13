using Microsoft.EntityFrameworkCore;

namespace TomAntillWebDevServices.Data.DataModels
{
    public class EmailSettings
    {
        public int Id { get; set; }
        public string Template { get; set; }
        public string WebsiteName { get; set; }
        public string EmailAddress { get; set; }

        public static void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<EmailSettings>();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.WebsiteName);
        }
    }
}
