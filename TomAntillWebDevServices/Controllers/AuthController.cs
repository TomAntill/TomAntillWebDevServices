using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using TomAntillWebDevServices.Attributes;
using TomAntillWebDevServices.Data.Auth.DataModels;
using TomAntillWebDevServices.Data.Enums;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;


namespace TomAntillWebDevServices.Controllers
{
    [ApiController]
    [EnableCors("AllowSpecificOrigins")]
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



        [HttpPost]
        [Route("ValidateToken")]
        public IActionResult ValidateToken([FromBody] string token, [FromQuery] string websiteName)
        
        {
            try
            {
                var validationResult = ValidateJwtToken(token, websiteName);
                return Ok(new { IsValid = validationResult });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsValid = false, ErrorMessage = ex.Message }); ;
            }
        }

        private bool ValidateJwtToken(string token, string websiteName)
        {

            try
            {
                if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = token.Substring(7);
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Authentication:SecretKey"]);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                // Validate the token
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out _);

                ///Get Website from token
                string webpage = claimsPrincipal.FindFirst(ClaimTypes.Webpage)?.Value;

                if (webpage == websiteName)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
