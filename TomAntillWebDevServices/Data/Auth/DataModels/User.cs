using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TomAntillWebDevServices.Data.Enums;

namespace TomAntillWebDevServices.Data.Auth.DataModels
{
    public class User
    {
        protected User()
        {
        }

        public User(string email, string passwordHash, string websiteName)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            UserSites.Add(new UserSite(websiteName, Id));
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }

        public virtual ICollection<UserSite> UserSites { get; set; } = new List<UserSite>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public static void Configure(ModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<User>()
                .ToTable("User");
            user.HasMany(u => u.UserSites).WithOne().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.UserRoles).WithOne().HasForeignKey(uc => uc.UserId);
            user.Property(u => u.Email).HasMaxLength(256);
        }
    }

    public class UserSite
    {
        protected UserSite()
        {
        }

        public UserSite(string websiteName, Guid userId)
        {
            WebsiteName = websiteName;
            UserId = userId;
        }

        public Guid Id { get; set; }
        public string WebsiteName { get; set; }

        public Guid UserId { get; set; }

        public static void Configure(ModelBuilder modelBuilder)
        {
            var userSite = modelBuilder.Entity<UserSite>()
                .ToTable("UserSite");
            userSite.Property(e => e.WebsiteName);
        }
    }

    public class UserRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid UserId { get; set; }

        public static void Configure(ModelBuilder modelBuilder)
        {
            var userRole = modelBuilder.Entity<UserRole>()
                .ToTable("UserRole");
            userRole.Property(u => u.Name).HasMaxLength(256);
        }
    }
}


