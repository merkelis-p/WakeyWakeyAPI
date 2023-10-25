using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WakeyWakeyAPI.Models
{
    public partial class Reminder
    {
        [Key]
        public int Id { get; set; }
        public DateTime ReminderDate { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public virtual Event? Event { get; set; }
    }
}
