using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TomAntillWebDevServices.Attributes;
using TomAntillWebDevServices.Data.Auth.DataModels;
using TomAntillWebDevServices.Data.Enums;

namespace TomAntillWebDevServices.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : JwtAuthController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userInManager;
        private static readonly object userCreationLock = new object();

        public AuthController(SignInManager<User> signInManager, UserManager<User> userInManager, IConfiguration configuration) : base(configuration)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userInManager = userInManager ?? throw new ArgumentNullException(nameof(userInManager));
        }

        [HttpGet]
        [Route("JwtLogin")]
        public async Task<IActionResult> JwtLogin(string userName, string password, string websiteName)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(userName, password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByNameAsync(userName);

                // Generate JWT token
                CheckUserHasWebsiteAccess(user, websiteName);
                var token = GenerateJwtToken(user, websiteName);

                // Return token and other response data
                return Ok(new { Token = token, user.Email, Site = websiteName.ToString() });
            }
            else
            {
                ModelState.AddModelError("Login", "Invalid login attempt.");
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("AddSystemUser")]
        [JwtAuthAttribute]
        public IActionResult AddSystemUser(string userName, string password, string websiteName)
        {          
            var signedInUser = _userInManager.FindByNameAsync(GetLoggedInUserName()).Result;
            
            if(!signedInUser.IsAdmin)
            {
                return BadRequest("User is not admin!");
            }

            lock (userCreationLock)
            {
                var existingUserWithUsername = _userInManager.FindByNameAsync(userName).Result;
                if (existingUserWithUsername != null)
                {
                    return BadRequest("Duplicate User!");
                }

                var user = _userInManager.CreateAsync(new User(userName, password, websiteName), password).Result;
                return user.Succeeded ? Ok(user.Succeeded) : BadRequest(user.Succeeded);
            }
        }
    }
}
