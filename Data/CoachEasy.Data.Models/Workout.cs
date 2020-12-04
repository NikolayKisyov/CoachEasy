namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using CoachEasy.Data.Common.Models;

    public class Workout : BaseDeletableModel<string>
    {
        public Workout()
        {
            this.Id = Guid.NewGuid().ToString();
            this.WorkoutsLists = new HashSet<WorkoutsList>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        [Required]
        public string Description { get; set; }

        [Required]
        [ForeignKey(nameof(Position))]
        public string PositionId { get; set; }

        [Required]
        public virtual Position Position { get; set; }

        [Required]
        public string VideoUrl { get; set; }

        public string ImageUrl { get; set; }

        public string PictureId { get; set; }

        public virtual Picture Picture { get; set; }

        public string CoachId { get; set; }

        public virtual Coach AddedByCoach { get; set; }

        public virtual ICollection<WorkoutsList> WorkoutsLists { get; set; }
    }
}
