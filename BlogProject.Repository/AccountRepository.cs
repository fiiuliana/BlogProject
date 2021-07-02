using BlogProject.Models.Account;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogProject.Repository
{
    public class AccountRepository : IAccountRepository
    {
        //installed package
        private readonly IConfiguration _config;

        //want to get the configuration for the Web application and from that config - want the database connection
        public AccountRepository(IConfiguration config) // will access the appsettings.json
        { 
            _config = config;   
        }
        
       
        public async Task<IdentityResult> CreateAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();


            //create a virtualtable
            var dataTable = new DataTable();

            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("NormalizedUsername", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("NormalizedEmail", typeof(string));
            dataTable.Columns.Add("Fullname", typeof(string));
            dataTable.Columns.Add("PasswordHash", typeof(string));

            dataTable.Rows.Add(
                user.Username,
                user.NormalizedUsername,
                user.Email,
                user.NormalizedEmail,
                user.Fullname,
                user.PasswordHash
                );

            //open a connection with SqlServer
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"))) 
            {
                //cancel if something goes wrong
                await connection.OpenAsync(cancellationToken);

                //insert if all ok
                await connection.ExecuteAsync("Account_Insert",      // account insert = stored procedure in SQL DB
                    new { Account = dataTable.AsTableValuedParameter("dbo.AccountType") },
                    commandType: CommandType.StoredProcedure);
            }
            return IdentityResult.Success;
        }
        
        public async Task<ApplicationUserIdentity> GetByUsernameAsync(string normalizedUsername, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ApplicationUserIdentity applicationUser;

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                //cancel if something goes wrong
                await connection.OpenAsync(cancellationToken);

                
                applicationUser = await connection.QuerySingleOrDefaultAsync<ApplicationUserIdentity>(
                    "Account_GetByUsername", 
                    new { NormalizedUsername = normalizedUsername },
                    commandType: CommandType.StoredProcedure
                    );             
            }
            return applicationUser;
        }
    }
}
