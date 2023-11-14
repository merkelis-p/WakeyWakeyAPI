using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WakeyWakeyAPI.Models
{
    public partial class Event
    {
        public Event()
        {
            Reminders = new HashSet<Reminder>();
            Name = string.Empty;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public int? Recurring { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<Reminder>? Reminders { get; set; }
    }
}
