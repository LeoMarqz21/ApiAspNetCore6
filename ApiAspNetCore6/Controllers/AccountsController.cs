using ApiAspNetCore6.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
                return await CreateToken(userCredentials);
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
                return await CreateToken(userCredentials);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }

        [HttpGet("renew-token")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AuthenticationResponse>> RenewToken()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var userCredentials = new UserCredentials()
            {
                Email = email
            };
            return await CreateToken(userCredentials);
        }

        private async Task<AuthenticationResponse> CreateToken(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", userCredentials.Email)
            };
            var user = await userManager.FindByEmailAsync(userCredentials.Email);
            var claimsDB = await userManager.GetClaimsAsync(user);
            claims.AddRange(claimsDB);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(30);
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

        //-------------------------------------------------------------------------

        [HttpPost("promote-to-admin")]
        public async Task<ActionResult> PromoteToAdmin(ProposedAdministrator admin)
        {
            var user = await userManager.FindByEmailAsync(admin.Email);
            await userManager.AddClaimAsync(user, new Claim("IsAdmin", "1"));
            return NoContent();
        }

        [HttpPost("revoke-admin")]
        public async Task<ActionResult> RevokeAdmin(ProposedAdministrator admin)
        {
            var user = await userManager.FindByEmailAsync(admin.Email);
            await userManager.RemoveClaimAsync(user, new Claim("IsAdmin", "1"));
            return NoContent();
        }

    }
}
