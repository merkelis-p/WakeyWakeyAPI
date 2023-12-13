    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace WakeyWakeyAPI.Models {

        public partial class Task
        {
            
            public Task()
            {
                SubTasks = new HashSet<Task>();
            }
            [Key]
            public int Id { get; set; }

            [Required]
            public int Category { get; set; }
            public int? ParentId { get; set; }

            [ForeignKey("Subject")]
            public int? SubjectId { get; set; }
            public virtual Subject? Subject { get; set; }

            [ForeignKey("User")]
            public int UserId { get; set; }
            public virtual User? User { get; set; }

            [Required]
            [MaxLength(255)]
            public string Name { get; set; }

            [MaxLength(5000)]
            public string? Description { get; set; }

            public int? EstimatedDuration { get; set; }
            public int? OverallDuration { get; set; }
            public DateTime? DeadlineDate { get; set; }
            public int? Score { get; set; }
            public int? ScoreWeight { get; set; }

            [Required]
            public int Status { get; set; }
            
            // Navigation property for the parent task
            public Task? ParentTask { get; set; }

            // Collection of subtasks
            public virtual ICollection<Task>? SubTasks { get; set; }

            public virtual ICollection<Record>? Records { get; set; }
        }

    }