using Microsoft.EntityFrameworkCore;
using TomAntillWebDevServices.Data.Auth.DataModels;
using TomAntillWebDevServices.Data.DataModels;

namespace TomAntillWebDevServices.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BasePicture> BasePicture { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<EmailSettings> EmailSettings { get; set; }

        // Auth
        public DbSet<User> Users { get; set; }
        public DbSet<UserSite> UserSites { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=TomAntillWebDev.db");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            DataModels.Email.Configure(modelBuilder);
            DataModels.BasePicture.Configure(modelBuilder);
            DataModels.EmailSettings.Configure(modelBuilder);
            User.Configure(modelBuilder);
            UserSite.Configure(modelBuilder);
            UserRole.Configure(modelBuilder);
        }
    }
}
