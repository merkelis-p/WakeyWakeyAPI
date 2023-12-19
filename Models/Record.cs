using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable

namespace WakeyWakeyAPI.Models
{
    public partial class Record
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Category { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Task")]
        public int? TaskId { get; set; }
        public virtual Task Task { get; set; }

        public string Note { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Duration { get; set; }
        public int? FocusDuration { get; set; }
        public int? BreakDuration { get; set; }
        public int? BreakFrequency { get; set; }
    }
}
