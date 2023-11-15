using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WakeyWakeyAPI.Models {

    public partial class Subject
    {
            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(255)]
            public string Name { get; set; }

            [MaxLength(5000)]
            public string? Description { get; set; }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            [Required]
            public int Status { get; set; }
            public int? Score { get; set; }
            public int? ScoreWeight { get; set; }

            [ForeignKey("Course")]
            public int CourseId { get; set; }
            public virtual Course Course { get; set; }

            public virtual ICollection<Task>? Tasks { get; set; }
    }

}