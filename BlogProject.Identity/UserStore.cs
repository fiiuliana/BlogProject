using BlogProject.Models.Account;
using BlogProject.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogProject.Identity
{

    public class UserStore :
        IUserStore<ApplicationUserIdentity>,
        IUserEmailStore<ApplicationUserIdentity>,
        IUserPasswordStore<ApplicationUserIdentity>
    {

        private readonly IAccountRepository _accountRepository;

        public UserStore(IAccountRepository accountRepository) {
            _accountRepository = accountRepository;
        }
        // implemented
        public async Task<IdentityResult> CreateAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            return await _accountRepository.CreateAsync(user, cancellationToken);

        }
        // implemented
        public async Task<ApplicationUserIdentity> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await _accountRepository.GetByUsernameAsync(normalizedUserName, cancellationToken);
        }


        // left as it is
        public Task<IdentityResult> DeleteAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        // left as it is
        public Task<ApplicationUserIdentity> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        // left as it is
        public Task<ApplicationUserIdentity> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

 
        // implemented
        public Task<string> GetEmailAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }
        // implemented
        public Task<bool> GetEmailConfirmedAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
        // implemented
        public Task<string> GetNormalizedEmailAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }
        // implemented
        public Task<string> GetNormalizedUserNameAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUsername);
        }

        // implemented
        public Task<string> GetPasswordHashAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
        return Task.FromResult(user.PasswordHash);
    }
        // implemented
        public Task<string> GetUserIdAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.ApplicationUserId.ToString());
        }
        // implemented
        public Task<string> GetUserNameAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Username);
        }
        //implemented
        public Task<bool> HasPasswordAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
        //implemented
        public Task SetEmailAsync(ApplicationUserIdentity user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }
        //implemented
        public Task SetEmailConfirmedAsync(ApplicationUserIdentity user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
        //implemented
        public Task SetNormalizedEmailAsync(ApplicationUserIdentity user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }
        //implemented
        public Task SetNormalizedUserNameAsync(ApplicationUserIdentity user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUsername = normalizedName;
            return Task.FromResult(0);
        }
        //implemented
        public Task SetPasswordHashAsync(ApplicationUserIdentity user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }
        //implemneted
        public Task SetUserNameAsync(ApplicationUserIdentity user, string userName, CancellationToken cancellationToken)
        {
            user.Username = userName;
            return Task.FromResult(0);
        }
        //not going to use - not implemented
        public Task<IdentityResult> UpdateAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // nothing to dispose
        }
    }
}
