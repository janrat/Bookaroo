using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bookaroo.Services;

namespace Bookaroo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase, IAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            if (login.Username == "admin" && login.Password == "admin")
            {
                var token = GenerateJwtToken(login.Username, "Admin");
                return Ok(new { token });
            }
            else if (login.Username == "user" && login.Password == "user")
            {
                var token = GenerateJwtToken(login.Username, "User");
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(string username, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        internal bool ValidateToken(string token, out ClaimsPrincipal principal)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));

            try
            {
                principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = secretKey,
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = false
                }, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception Ex)
            {
                var test = Ex;

                principal = null;
                return false;
            }
        }

        internal bool ValidateUserRole(string token, string requiredRole)
        {
            if (ValidateToken(token, out ClaimsPrincipal principal))
            {
                var roleClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
                if (roleClaim != null && roleClaim.Value == requiredRole)
                {
                    return true;
                }
            }

            return false;
        }

        bool IAuthenticationService.ValidateToken(string token, out ClaimsPrincipal principal)
        {
            principal = null;
            return ValidateToken(token, out principal);
        }

        bool IAuthenticationService.ValidateUserRole(string token, string role)
        {
            return ValidateUserRole(token, role);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
