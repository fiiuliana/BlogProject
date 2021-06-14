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

        // got ot Start-up - redirect to toen service
        public AccountController(ITokenService tokenService, 
            UserManager<ApplicationUserIdentity> userManager,
            SignInManager<ApplicationUserIdentity> signInManager) 
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // http://localhost:4200/api/Account/register - most likely
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> Register(ApplicationUserCreate applicationUserCreate)
        {
            //has the Hash password
            var applicationUserIdentity = new ApplicationUserIdentity
            {
                Username = applicationUserCreate.Username,
                Email = applicationUserCreate.Email,
                Fullname = applicationUserCreate.Fullname
            };

            // send back the pass - not a hashed pass
            var result = await _userManager.CreateAsync(applicationUserIdentity, applicationUserCreate.Password);
            if (result.Succeeded)
            {
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    ApplicationUserId = applicationUserIdentity.ApplicationUserId,
                    UserName = applicationUserIdentity.Username,
                    Email = applicationUserIdentity.Email,
                    Fulname = applicationUserIdentity.Fullname,
                    Token = _tokenService.CreateToken(applicationUserIdentity)
                };
                return Ok(applicationUser);
            }
            return BadRequest(result.Errors);
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApplicationUser>> Login(ApplicationUserLogin applicationUserLogin)
        {
            var applicationUserIdentity = await _userManager.FindByNameAsync(applicationUserLogin.Username);

            // got the user back - check it
            if (applicationUserIdentity != null) 
            {
                var result = await _signInManager.CheckPasswordSignInAsync(
                    applicationUserIdentity,
                    applicationUserLogin.Password, false);

                // user signed in - was it successfull?
                if (result.Succeeded) 
                {
                    ApplicationUser applicationUser = new ApplicationUser()
                    {
                        ApplicationUserId = applicationUserIdentity.ApplicationUserId,
                        UserName = applicationUserIdentity.Username,
                        Email = applicationUserIdentity.Email,
                        Fulname = applicationUserIdentity.Fullname,
                        Token = _tokenService.CreateToken(applicationUserIdentity)
                    };
                  return Ok(applicationUser);
                }
            }
            return BadRequest("Invalid Login Attempt");
        }
    }
}
