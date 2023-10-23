using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WakeyWakeyAPI.Models {

    public partial class Course
    {
            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(255)]
            public string Name { get; set; }

            [MaxLength(5000)]
            public string? Description { get; set; }

            [Required]
            public DateTime StartDate { get; set; }
            
            [Required]
            public DateTime EndDate { get; set; }

            [Required]
            public int Status { get; set; }
            public int? Score { get; set; }

            [ForeignKey("User")]
            public int UserId { get; set; }
            public virtual User? User { get; set; }

            public virtual ICollection<Subject>? Subjects { get; set; }
    }

}