using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WakeyWakeyAPI.Models;
using System.Security.Cryptography;
using System.Text;
using WakeyWakeyAPI.Repositories;

namespace WakeyWakeyAPI.Controllers
{
    public class UserController : GenericController<User, IUserRepository>, IUserController
    {
        readonly IUserRepository _context;
        public UserController(IUserRepository repository) : base(repository)
        {
            _context = repository;
        }

        // POST: api/Users/Login
        [HttpPost("Login")]
        public async Task<ActionResult<LoginValidationResult>> ValidateLogin(UserLoginRequest loginRequest)
        {
            var user = await _context.GetByUsernameAsync(loginRequest.Username);
            if (user == null)
            {
                return NotFound();
            }

            using var hmac = new HMACSHA512(Convert.FromBase64String(user.Salt));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

            if (!computedHash.SequenceEqual(Convert.FromBase64String(user.Password)))
            {
                return Unauthorized(); 
            }

            return Ok(new LoginValidationResult 
            {
                IsValid = true,
                UserId = user.Id
            });
        }

        
        
        // POST: api/Users/Register
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserRegisterRequest registerRequest)
        {
            if (await _context.ExistsAsync(registerRequest.Username) ||
                await _context.ExistsEmailAsync(registerRequest.Email))
            {
                return BadRequest("Username or email already exists.");
            }

            using var hmac = new HMACSHA512();
            var user = new User
            {
                Username = registerRequest.Username,
                Salt = Convert.ToBase64String(hmac.Key),
                Email = registerRequest.Email,
                Password = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(registerRequest.Password)))
            };

            await _context.AddAsync(user);
            return CreatedAtAction("GetById", new { id = user.Id }, user);
        }
        
        
        
     
        
    }
}
