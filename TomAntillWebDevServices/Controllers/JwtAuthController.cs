using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using TomAntillWebDevServices.Attributes;
using TomAntillWebDevServices.Data.Auth.DataModels;
using TomAntillWebDevServices.Data.Enums;

namespace TomAntillWebDevServices.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class JwtAuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public JwtAuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ValidateToken();
            base.OnActionExecuting(context);
        }

        protected void CheckSystemPasswordIsValid()
        {
            var systemPassword = _configuration["Authentication:SystemAdminPassword"];
            HttpContext.Request.Headers.TryGetValue("SystemPassword", out var password);
            if (systemPassword != password) throw new Exception("Invalid system login request");
        }

        protected void CheckUserHasWebsiteAccess(User user, string websiteName)
        {
            if (!user.UserSites.Any(a => a.WebsiteName == websiteName)) throw new Exception("Access to website denied"); ;
        }

        protected string GenerateJwtToken(User user, string websiteName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Authentication:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Webpage, websiteName.ToString())
            }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        protected void ValidateToken()
        {
            var destinationMethod = GetFunctionForRequest();
            if (FunctionHasJwtAuthAttribute(destinationMethod))
            {
                bool hasToken = HttpContext.Request.Headers.TryGetValue("Authorization", out var tokenValues);
                if (!hasToken) throw new ArgumentNullException("Authorization token not found");

                string token = tokenValues[0];

                if (token.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
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
                    ValidateAudience = false
                };

                tokenHandler.ValidateToken(token, validationParameters, out _);
            }
        }

        protected string GetLoggedInUserName()
        {
            bool hasToken = HttpContext.Request.Headers.TryGetValue("Authorization", out var tokenValues);
            if (!hasToken) throw new ArgumentNullException("Authorization token not found");

            string token = tokenValues[0];

            if (token.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
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
                ValidateAudience = false
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal.Identity.Name;
        }

        private MethodInfo GetFunctionForRequest() => ControllerContext.ActionDescriptor.MethodInfo;

        private bool FunctionHasJwtAuthAttribute(MethodInfo methodInfo) => methodInfo.IsDefined(typeof(JwtAuthAttribute), true);
    }
}
