using BlogProject.Models.Account;
using BlogProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Web.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
        public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUserIdentity> _userManager;
        private readonly SignInManager<ApplicationUserIdentity> _signInManager;

        // go tot Start-up - redirect to token service
        //constructor
        public AccountController(ITokenService tokenService, 
            UserManager<ApplicationUserIdentity> userManager,
            SignInManager<ApplicationUserIdentity> signInManager) 
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // http://localhost:4200/api/Account/register - most likely
        /// <summary>
        /// Register - Endpoint - POST request
        /// </summary>
        /// <param name="applicationUserCreate">the applicationUserCreate</param>
        /// <returns>Application user</returns>
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> Register(ApplicationUserCreate applicationUserCreate)
        {    
            // For user creation the plain text password is needed 

            // On the Identity the plain text is not needed, therefore it has the Hash password
            var applicationUserIdentity = new ApplicationUserIdentity
            {
                Username = applicationUserCreate.Username,
                Email = applicationUserCreate.Email,
                Fullname = applicationUserCreate.Fullname
            };

            // the password is sent separately - it gets hashed and then put on the Identity
            // send back the pass - not a hashed pass
            var result = await _userManager.CreateAsync(applicationUserIdentity, applicationUserCreate.Password);
            
            //the user is created in database and a result is received
            if (result.Succeeded)
            {
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    ApplicationUserId = applicationUserIdentity.ApplicationUserId,
                    Username = applicationUserIdentity.Username,
                    Email = applicationUserIdentity.Email,
                    Fulname = applicationUserIdentity.Fullname,
                    // we do not want to send a hass password;
                    // the token sent back to user will receive the applicationUserIdentity
                    Token = _tokenService.CreateToken(applicationUserIdentity)
                };
                return Ok(applicationUser);
            }
            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Login - the second endpoint - POST request
        /// </summary>
        /// <param name="applicationUserLogin">applicationUserLogin</param>
        /// <returns>the applicationUser</returns>
        [HttpPost("login")]
        public async Task<ActionResult<ApplicationUser>> Login(ApplicationUserLogin applicationUserLogin)
        {
            //take the applicationUserLogin / username
            var applicationUserIdentity = await _userManager.FindByNameAsync(applicationUserLogin.Username);

            // got the user back - check it
            if (applicationUserIdentity != null) 
            {   
                //
                var result = await _signInManager.CheckPasswordSignInAsync(
                    applicationUserIdentity,
                    applicationUserLogin.Password, false);

                // user signed in - was it successfull?
                if (result.Succeeded) 
                {
                    ApplicationUser applicationUser = new ApplicationUser()
                    {
                        ApplicationUserId = applicationUserIdentity.ApplicationUserId,
                        Username = applicationUserIdentity.Username,
                        Email = applicationUserIdentity.Email,
                        Fulname = applicationUserIdentity.Fullname,
                        Token = _tokenService.CreateToken(applicationUserIdentity)
                    };
                  return Ok(applicationUser);
                }
            }
            // the user was invalid
            return BadRequest("Incercare de logare invalida");
        }
    }
}
