using Microsoft.AspNetCore.Identity;
using System;
using TomAntillWebDevServices.Data.Auth.DataModels;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace TomAntillWebDevServices.Data.Auth.Stores
{
    public class UserStore : IUserStore<User>, IUserPasswordStore<User>
    {
        private AppDbContext _context = null;
        public bool disposeContext { get; set; }

        public UserStore(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            this.disposeContext = true;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Code = "CreateUserException", Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Code = "DeleteUserException", Description = ex.Message });
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposeContext && disposing && _context != null)
            {
                _context.Dispose();
            }
            _context = null;
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken) => await _context.Users.FirstOrDefaultAsync(s => s.Id.ToString() == userId);

        public async Task<User> FindByNameAsync(string emailAddress, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(inc => inc.UserRoles)
                .Include(inc => inc.UserSites)
                .SingleOrDefaultAsync(s => s.Email.ToLower() == emailAddress.ToLower());
            return user;
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email = normalizedName);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email = userName);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            try
            {
                var entity = await _context.Users.SingleAsync(s => s.Id == user.Id);
                entity.Email = user.Email;
                entity.PasswordHash = user.PasswordHash;
                await _context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Code = "CreateUserException", Description = ex.Message });
            }
        }

        private void ThrowIfDisposed()
        {
            if (_context is null)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
    }
}
