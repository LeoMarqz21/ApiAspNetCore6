using ApiAspNetCore6.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiAspNetCore6.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;

        public AccountsController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Register(UserCredentialsRegister userCredentials)
        {
            var user = new IdentityUser
            {
                UserName = userCredentials.UserName,
                Email = userCredentials.Email,
            };
            var result = await userManager.CreateAsync(user, userCredentials.Password);
            if(result.Succeeded)
            {
                return CreateToken(userCredentials);
            }else
            {
                return BadRequest(result.Errors);
            }
        }

        private AuthenticationResponse CreateToken(UserCredentialsRegister userCredentialsRegister)
        {
            var claims = new List<Claim>()
            {
                new Claim("UserName", userCredentialsRegister.UserName),
                new Claim("email", userCredentialsRegister.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddYears(1);
            var securityToken = new JwtSecurityToken(
                issuer: null, 
                audience: null, 
                claims: claims, 
                expires: expires,
                signingCredentials: credentials
                );
            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expires = expires
            };
        }

    }
}
