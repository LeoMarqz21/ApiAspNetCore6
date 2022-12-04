using ApiAspNetCore6.DTOs;
using Microsoft.AspNetCore.Authorization;
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
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountsController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponse>> Register(UserCredentials userCredentials)
        {
            var user = new IdentityUser
            {
                UserName = userCredentials.Email,
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResponse>> Login(UserCredentials userCredentials)
        {
            var result = await signInManager
                .PasswordSignInAsync(
                userCredentials.Email,
                userCredentials.Password,
                isPersistent: false,
                lockoutOnFailure: false
                );
            if(result.Succeeded)
            {
                return CreateToken(userCredentials);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }

        private AuthenticationResponse CreateToken(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", userCredentials.Email)
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
