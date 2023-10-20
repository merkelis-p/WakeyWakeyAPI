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
    public class UserController : GenericController<User, UserRepository>
    {
        readonly UserRepository _context;
        public UserController(UserRepository repository) : base(repository)
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

            return new LoginValidationResult 
            {
                IsValid = computedHash.SequenceEqual(Convert.FromBase64String(user.Password)),
                UserId = user.Id
            };
        }

        
    }
}
