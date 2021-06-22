using BlogProject.Models.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogProject.Repository
{
    public interface IAccountRepository
    {
        //the initial result is going to be a "promise" - we will get the result after some time
        public Task<IdentityResult> CreateAsync(ApplicationUserIdentity user, CancellationToken cancellationToken);

        //just for the account repository - given if the registration process stops
        public Task<ApplicationUserIdentity> GetByUsernameAsync(string normalizedUsername,
            CancellationToken cancellationToken);   //.Net core Identity gives this is somethig is cancelled
    }
}
