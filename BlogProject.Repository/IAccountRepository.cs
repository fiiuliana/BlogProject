using BlogProject.Models.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogProject.Repository
{
    public interface IAccountRepository
    {
        //the result is going to be a "promise" - asynk
        public Task<IdentityResult> CreateAsync(ApplicationUserIdentity user, CancellationToken cancellationToken);

        public Task<ApplicationUserIdentity> GetByUsernameAsync(string normalizedUsername,
            CancellationToken cancellationToken);  //just for the account repository - given
                                                   //if the registration process stops
    }
}
