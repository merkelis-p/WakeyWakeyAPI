using System.ComponentModel.DataAnnotations;

namespace WakeyWakeyAPI.Models
{
    public class UserLoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}