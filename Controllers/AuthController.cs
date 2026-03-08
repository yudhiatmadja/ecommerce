using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleApiCrud.Data;
using SimpleApiCrud.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;


namespace SimpleApiCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context , IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
             var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
             if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
             {
                 return Unauthorized("Invalid username or password");
             }

             var tokenHandler = new JwtSecurityTokenHandler();
             var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
             var tokenDescriptor = new SecurityTokenDescriptor
             {
                 Subject = new ClaimsIdentity(new Claim[]
                 {
                     new Claim(ClaimTypes.Name, user.Username),
                     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                 }),
                 Expires = DateTime.UtcNow.AddHours(1),
                 SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
             };
             var token = tokenHandler.CreateToken(tokenDescriptor);
             var tokenString = tokenHandler.WriteToken(token);

             return Ok(new LoginResponse { Token = tokenString });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists");
            }

            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = request.Password
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();  

            return Ok("User registered successfully");
        }
    }
}