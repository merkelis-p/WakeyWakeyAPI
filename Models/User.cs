using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;


#nullable disable

namespace WakeyWakeyAPI.Models
{
    public partial class User
    {
        public User()
        {
            Courses = new HashSet<Course>();
            Events = new HashSet<Event>();
            Records = new HashSet<Record>();
            Tasks = new HashSet<Task>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } 

        [MaxLength(255)]
        public string Salt { get; set; }


        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Record> Records { get; set; }
    }

}