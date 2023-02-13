using JWT_Login.Helpers;
using JWT_Login.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JWT_Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly LoginJwtContext _context;

        public AuthController(LoginJwtContext context, IConfiguration configuration) {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(string username, string password)
        {
            try
            {
                var user = _context.Users.Where(user => user.Username == username);
                if (user.IsNullOrEmpty()) return NotFound(ApiResponesHelper.GetApiResponses("User not found", false));


                return Ok();
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(string username, string password, string confirmPassword)
        {
            if(password != confirmPassword)
            {
                return BadRequest(ApiResponesHelper.GetApiResponses("Passwords must be equals", false));
            }

            var userExist = _context.Users.Where(u => u.Username == username);
            if (!userExist.IsNullOrEmpty())
            {
                return BadRequest(ApiResponesHelper.GetApiResponses("User already register", false));
            }

            User newUser = new User() { Username = username, Password = Encrypt.GetSHA256(password) };
            _context.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(ApiResponesHelper.GetApiResponses("Register success", true));

        }
    }
}
